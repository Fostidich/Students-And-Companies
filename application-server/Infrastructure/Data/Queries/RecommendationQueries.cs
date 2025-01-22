using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class RecommendationQueries : IRecommendationQueries {
    
    private readonly IDataService dataService;

    public RecommendationQueries(IDataService dataService) {
        this.dataService = dataService;
    }
    
    public List<Entity.Advertisement> GetAdvertisementsOfCompany(int studentId) {
        try {
            string query = @"
                SELECT *
                FROM advertisements a
                WHERE a.company_id = @CompanyId;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@StudentId", studentId);
            
            using var reader = command.ExecuteReader();
            
            var advertisements = dataService.MapToAdvertisements(reader);
            return advertisements;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
     
    
    public List<Entity.Advertisement> GetAdvertisementsForStudent(int studentId) {
        try {
            string query = @"
                SELECT
                    a.id AS advertisement_id,
                    a.created_at,
                    a.company_id,
                    a.description,
                    a.duration,
                    a.spots,
                    a.available,                   
                    a.open,
                    a.questionnaire,
                FROM advertisement_skills ads
                INNER JOIN advertisement a ON ads.advertisemen_id = a.id
                INNER JOIN student_skills ss ON ads.skill_id = ss.skill_id
                WHERE ss.student_id = @StudentId
                  AND a.open = TRUE -- Consider only open advertisements
                GROUP BY a.id, a.created_at, a.company_id, a.description, a.duration, a.spots, a.available, a.open, a.questionnaire 
                ORDER BY COUNT(*) DESC
                LIMIT 100;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@StudentId", studentId);
            
            using var reader = command.ExecuteReader();
            
            var advertisements = dataService.MapToAdvertisements(reader);
            return advertisements;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    public int? CreateAdvertisement(int companyId, Entity.Advertisement advertisement, List<Entity.Skill> skills) {
        try {
            string query = @"
                INSERT INTO advertisement (company_id, description, duration, spots, available, open, questionnaire)
                VALUES (@CompanyId, @Description, @Duration, @Spots, @Avaible, @Open, @Questionnaire);
                SELECT LAST_INSERT_ID();
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@CompanyId", companyId);
            command.Parameters.AddWithValue("@Description", advertisement.Description);
            command.Parameters.AddWithValue("@Duration", advertisement.Duration);
            command.Parameters.AddWithValue("@Spots", advertisement.Spots);
            command.Parameters.AddWithValue("@Available", advertisement.Spots);
            command.Parameters.AddWithValue("@Open", "TRUE"); 
            command.Parameters.AddWithValue("@Questionnaire", advertisement.Questionnaire);
            
            using var reader = command.ExecuteReader();

            if (reader.Read()) {
                int advId = Convert.ToInt32(reader[0]);
                List<int> skillId = AddSkillsIfNotAlreadyPresent(skills); // Add skills if they do not exist
                AddSkillsToAdvertisement(advId, skillId); // Link skills to advertisement
                return advId; // Return the ID of the newly created advertisement
            }

            return null;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    private List<int> AddSkillsIfNotAlreadyPresent(List<Entity.Skill> skills) {
        
        var skillIds = new List<int>();

        try {
            string insertQuery = @"
            INSERT IGNORE INTO skills (name)
            VALUES (@Name);
        ";
            string selectQuery = @"
            SELECT skill_id FROM skills WHERE name = @Name;
        ";

            using var db_connection = dataService.GetConnection();

            foreach (var skill in skills) {
                // Insert skill (ignored if already exists)
                using (var insertCommand = new MySqlCommand(insertQuery, db_connection)) {
                    insertCommand.Parameters.AddWithValue("@Name", skill.Name);
                    insertCommand.ExecuteNonQuery();
                }

                // Retrieve the skill ID
                using (var selectCommand = new MySqlCommand(selectQuery, db_connection)) {
                    
                    selectCommand.Parameters.AddWithValue("@Name", skill.Name);
                    
                    var skillId = selectCommand.ExecuteScalar();
                    
                    skillIds.Add(Convert.ToInt32(skillId));

                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding or retrieving skills: {ex.Message}");
        }

        return skillIds;
    }

    
    private void AddSkillsToAdvertisement(int advertisementId, List<int> skillIds)
    {
        try
        {
            string query = @"
            INSERT INTO advertisement_skills (advertisement_id, skill_id)
            VALUES (@AdvertisementId, @SkillId);
        ";

            using var db_connection = dataService.GetConnection();

            foreach (var skillId in skillIds)
            {
                using var command = new MySqlCommand(query, db_connection);
                command.Parameters.AddWithValue("@AdvertisementId", advertisementId);
                command.Parameters.AddWithValue("@SkillId", skillId);
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error associating skills to advertisement: {ex.Message}");
        }
    }
    
    public void MatchAdvertisementForStudent(int advertisementId)
    {
        try {
            // Query to find students with at least 3 skill matches
            string findStudentsQuery = @"
            SELECT
                ss.student_id
            FROM student_skills ss
            INNER JOIN advertisement_skills ads ON ss.skill_id = ads.skill_id
            WHERE ads.advertisement_id = @AdvertisementId
            GROUP BY ss.student_id
            HAVING COUNT(*) >= 3;
        ";

            // Query to insert notifications
            string insertNotificationQuery = @"
            INSERT INTO StudentNotifications (student_id, advertisement_id, type)
            VALUES (@StudentId, @AdvertisementId, 'r');
        ";

            using var db_connection = dataService.GetConnection();

            // Find matching students
            List<int> matchingStudentIds = new List<int>();
            using (var findCommand = new MySqlCommand(findStudentsQuery, db_connection)) {
                
                findCommand.Parameters.AddWithValue("@AdvertisementId", advertisementId);
                using var reader = findCommand.ExecuteReader();
                
                while (reader.Read()) {
                    matchingStudentIds.Add(Convert.ToInt32(reader["StudentId"]));
                }
            }

            // Insert notifications for matching students
            foreach (var studentId in matchingStudentIds) {
                using var insertCommand = new MySqlCommand(insertNotificationQuery, db_connection);
                insertCommand.Parameters.AddWithValue("@StudentId", studentId);
                insertCommand.Parameters.AddWithValue("@AdvertisementId", advertisementId);
                insertCommand.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error matching advertisement to students: {ex.Message}");
        }
    }
    
    public void MatchAdvertisementForCompany(int advertisementId)
    {
        try {
            // Query to find students with at least 3 matching skills for the given advertisement
            string findStudentsQuery = @"
                SELECT
                    ss.student_id
                FROM student_skills ss
                INNER JOIN advertisement_skills ads ON ss.skill_id = ads.skill_id
                WHERE ads.advertisement_id = @advertisement_id
                GROUP BY ss.student_id
                HAVING COUNT(*) >= 3;
            ";

            // Query to find the company associated with the advertisement
            string findCompanyQuery = @"
                SELECT company_id
                FROM advertisements
                WHERE id = @advertisement_id;
            ";

            // Query to insert notifications into the company_notifications table
            string insertNotificationQuery = @"
                INSERT INTO company_notifications (company_id, student_id, advertisement_id, type)
                VALUES (@company_id, @student_id, @advertisement_id, 'r');
            ";

            using var db_connection = dataService.GetConnection();

            // Retrieve the company ID associated with the given advertisement
            int companyId;
            using (var companyCommand = new MySqlCommand(findCompanyQuery, db_connection)) {
                
                companyCommand.Parameters.AddWithValue("@advertisement_id", advertisementId);
                var result = companyCommand.ExecuteScalar();
                
                if (result == null) {
                    Console.WriteLine("No company found for the advertisement.");
                    return;
                }
                
                companyId = Convert.ToInt32(result); // Convert the result to an integer
            }

            // Find students with at least 3 matching skills for the advertisement
            List<int> matchingStudentIds = new List<int>();
            using (var findCommand = new MySqlCommand(findStudentsQuery, db_connection)) {
                
                findCommand.Parameters.AddWithValue("@advertisement_id", advertisementId);
                using var reader = findCommand.ExecuteReader();
                
                while (reader.Read()) {
                    matchingStudentIds.Add(Convert.ToInt32(reader["student_id"])); // Add student IDs to the list
                }
            }

            // Insert notifications into the company_notifications table for matching students
            foreach (var studentId in matchingStudentIds) {
                using var insertCommand = new MySqlCommand(insertNotificationQuery, db_connection);
                insertCommand.Parameters.AddWithValue("@company_id", companyId); // Associate with the company
                insertCommand.Parameters.AddWithValue("@student_id", studentId); // Associate with the student
                insertCommand.Parameters.AddWithValue("@advertisement_id", advertisementId); // Associate with the advertisement
                insertCommand.ExecuteNonQuery(); // Execute the insertion
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error matching advertisement to company: {ex.Message}");
        }
    }

    

}

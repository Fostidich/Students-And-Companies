using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

public class RecommendationQueries : IRecommendationQueries {
    
    private readonly IDataService dataService;

    public RecommendationQueries(IDataService dataService) {
        this.dataService = dataService;
    }
    
    public List<Entity.Advertisement> GetAdvertisementsOfCompany(int companyId) {
        try {
            string query = @"
                SELECT *
                FROM advertisement a
                WHERE a.company_id = @CompanyId;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@CompanyId", companyId);
            
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
                    a.advertisement_id,
                    a.created_at,
                    a.company_id,
                    a.description,
                    a.duration,
                    a.spots,
                    a.available,                   
                    a.open,
                    a.questionnaire
                FROM advertisement_skills ads
                INNER JOIN advertisement a ON ads.advertisement_id = a.advertisement_id
                INNER JOIN student_skills ss ON ads.skill_id = ss.skill_id
                WHERE ss.student_id = @StudentId
                  AND a.open = true
                GROUP BY a.advertisement_id, a.created_at, a.company_id, a.description, a.duration, a.spots, a.available, a.open, a.questionnaire 
                ORDER BY COUNT(*) DESC
                LIMIT 40;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@StudentId", studentId);
            
            using var reader = command.ExecuteReader();
            
            var advertisements = dataService.MapToAdvertisements(reader);
            
            
            // If there are less than 40 advertisements, add default advertisements
            List<Entity.Advertisement> defaultAdvertisements = new List<Entity.Advertisement>();
            if (advertisements.Count < 40) {
                defaultAdvertisements = GetDefaultAdvertisements(advertisements);
            }

            advertisements.Concat(defaultAdvertisements);
            
            return advertisements;
            
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    private List<Entity.Advertisement> GetDefaultAdvertisements(List<Entity.Advertisement> advertisements) {
        try {
            string query = @"
                SELECT *
                FROM advertisement
                WHERE open = true
                LIMIT 40;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            using var reader = command.ExecuteReader();
            
            var defaultAdvertisements = dataService.MapToAdvertisements(reader);
            
            return defaultAdvertisements.Except(advertisements).ToList();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    
    
    public int? CreateAdvertisement(int companyId, Entity.Advertisement advertisement, List<Entity.Skill> skills) {
        try {
            string insertquery = @"
                INSERT INTO advertisement (company_id, description, duration, spots, available, open, questionnaire)
                VALUES (@CompanyId, @Description, @Duration, @Spots, @Available, @Open, @Questionnaire);
            ";
            
            string selectquery = @"
                SELECT MAX(advertisement_id) FROM advertisement;
            ";
            
            
            
            using var db_connection = dataService.GetConnection();
            
            // Insert the advertisement
            using (var insertcommand = new MySqlCommand(insertquery, db_connection)) {

                insertcommand.Parameters.AddWithValue("@CompanyId", companyId);
                insertcommand.Parameters.AddWithValue("@Description", advertisement.Description);
                insertcommand.Parameters.AddWithValue("@Duration", advertisement.Duration);
                insertcommand.Parameters.AddWithValue("@Spots", advertisement.Spots);
                insertcommand.Parameters.AddWithValue("@Available", advertisement.Spots);
                insertcommand.Parameters.AddWithValue("@Open", true);
                insertcommand.Parameters.AddWithValue("@Questionnaire", advertisement.Questionnaire);
                
                insertcommand.ExecuteNonQuery();

            }
            
            // Retrieve the advertisement ID
            using var command = new MySqlCommand(selectquery, db_connection);
            

            // Execute the query and retrieve the advertisement ID
            var result = command.ExecuteScalar();
            
            if (result != null) {
                int advId = Convert.ToInt32(result);

                // Add skills if they do not already exist
                List<int> skillIds = AddSkillsIfNotAlreadyPresent(skills);

                // Link skills to the advertisement
                AddSkillsToAdvertisement(advId, skillIds);

                return advId; // Return the advertisement ID
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
            INSERT IGNORE INTO skill (name)
            VALUES (@Name);
        ";
            string selectQuery = @"
            SELECT skill_id FROM skill WHERE name = @Name;
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

    
    private void AddSkillsToAdvertisement(int advertisementId, List<int> skillIds) {
        try {
            string query = @"
            INSERT INTO advertisement_skills (advertisement_id, skill_id)
            VALUES (@AdvertisementId, @SkillId);
        ";

            using var db_connection = dataService.GetConnection();

            foreach (var skillId in skillIds) {
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
                INSERT INTO company_notifications (company_id, student_id, advertisement_id)
                VALUES (@company_id, @student_id, @advertisement_id);
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
    
    public Entity.Advertisement GetAdvertisement(int advertisementId) {
        try {
            string query = @"
                SELECT *
                FROM advertisement
                WHERE advertisement_id = @AdvertisementId;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@AdvertisementId", advertisementId);
            
            using var reader = command.ExecuteReader();
            
            var advertisement = dataService.MapToAdvertisements(reader).FirstOrDefault();
            return advertisement;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public List<Entity.Student> GetRecommendedCandidates(int companyId, int advertisementId) {
        try {
            // Query to retrieve student IDs from company_notifications
            string getStudentIdsQuery = @"
                SELECT student_id
                FROM company_notifications
                WHERE company_id = @CompanyId AND advertisement_id = @AdvertisementId;
            ";

            // Query to retrieve student details
            string getStudentDetailsQuery = @"
                SELECT *
                FROM student
                WHERE student_id = @StudentId;
            ";
            
            // Check if company is the owner of the advertisement
            var advertisement = GetAdvertisement(advertisementId);
            if (advertisement == null || advertisement.CompanyId != companyId) {
                return null;
            }

            using var db_connection = dataService.GetConnection();

            // Step 1: Retrieve student IDs
            List<int> studentIds = new List<int>();
            using (var getIdsCommand = new MySqlCommand(getStudentIdsQuery, db_connection)) {
                getIdsCommand.Parameters.AddWithValue("@CompanyId", companyId);
                getIdsCommand.Parameters.AddWithValue("@AdvertisementId", advertisementId);

                using var reader = getIdsCommand.ExecuteReader();
                while (reader.Read())
                {
                    studentIds.Add(Convert.ToInt32(reader["student_id"]));
                }
            }

            // Step 2: Retrieve student details
            List<Entity.Student> students = new List<Entity.Student>();
            foreach (var studentId in studentIds) {
                using (var getDetailsCommand = new MySqlCommand(getStudentDetailsQuery, db_connection)) {
                    
                    getDetailsCommand.Parameters.AddWithValue("@StudentId", studentId);

                    using var reader = getDetailsCommand.ExecuteReader();
                    
                    var student = dataService.MapToStudents(reader).FirstOrDefault();
                    
                    students.Add(student);
                }
            }

            return students;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving recommended candidates: {ex.Message}");
            return new List<Entity.Student>();
        }
    }

    public bool CreateSuggestionsForStudent(int notificationId, int companyId) {
        try {
            // Query to retrieve the notification details from company_notifications
            string retrieveNotificationQuery = @"
                SELECT student_id, advertisement_id
                FROM company_notifications
                WHERE company_notification_id = @NotificationId AND company_id = @CompanyId;
            ";

            // Query to insert the notification into student_notifications
            string insertNotificationQuery = @"
                INSERT INTO student_notifications (student_id, advertisement_id, type)
                VALUES (@StudentId, @AdvertisementId, 'c');
            ";

            using var db_connection = dataService.GetConnection();

            // Retrieve the notification details
            int studentId, advertisementId;
            using (var retrieveCommand = new MySqlCommand(retrieveNotificationQuery, db_connection)) {
                retrieveCommand.Parameters.AddWithValue("@NotificationId", notificationId);
                using var reader = retrieveCommand.ExecuteReader();
                
                if (!reader.Read()) {
                    Console.WriteLine($"No company notification found with ID {notificationId} for your company.");
                    return false;
                }

                // Extract values from the result
                studentId = Convert.ToInt32(reader["student_id"]);
                advertisementId = Convert.ToInt32(reader["advertisement_id"]);
            }

            // Insert the notification into student_notifications
            using (var insertCommand = new MySqlCommand(insertNotificationQuery, db_connection)) {
                
                insertCommand.Parameters.AddWithValue("@StudentId", studentId);
                insertCommand.Parameters.AddWithValue("@AdvertisementId", advertisementId);
                
                insertCommand.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating suggestion for student: {ex.Message}");
            return false;
        }
    }

}

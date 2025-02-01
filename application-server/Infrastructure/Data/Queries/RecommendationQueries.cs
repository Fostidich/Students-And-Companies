using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

public class RecommendationQueries : IRecommendationQueries
{

    private readonly IDataService dataService;

    public RecommendationQueries(IDataService dataService)
    {
        this.dataService = dataService;
    }

    public List<Entity.Advertisement> GetAdvertisementsOfCompany(int companyId)
    {
        try
        {
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }


    public List<Entity.Advertisement> GetAdvertisementsForStudent(int studentId)
    {
        try
        {
            string query = @"
                SELECT a.advertisement_id, a.name, a.created_at, a.company_id, a.description, a.duration, a.spots, a.available, a.open, a.questionnaire
                FROM advertisement_skills ads
                JOIN advertisement a ON ads.advertisement_id = a.advertisement_id
                JOIN student_skills ss ON ads.skill_id = ss.skill_id
                WHERE ss.student_id = @StudentId
                  AND a.open = true
                GROUP BY a.advertisement_id, a.name, a.created_at, a.company_id, a.description, a.duration, a.spots, a.available, a.open, a.questionnaire
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
            if (advertisements.Count < 40)
            {
                defaultAdvertisements = GetDefaultAdvertisements(advertisements);
            }

            var advs = advertisements.Concat(defaultAdvertisements).ToList();

            return advs;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    private List<Entity.Advertisement> GetDefaultAdvertisements(List<Entity.Advertisement> advertisements)
    {
        try
        {
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

            var advs = defaultAdvertisements.Except(advertisements).ToList();
            
            return advs;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }



    public int? CreateAdvertisement(int companyId, Entity.Advertisement advertisement, List<Entity.Skill> skills)
    {
        try
        {
            string insertquery = @"
                INSERT INTO advertisement (company_id, description, name, duration, spots, available, open, questionnaire)
                VALUES (@CompanyId, @Description, @Name, @Duration, @Spots, @Available, @Open, @Questionnaire);
            ";

            string selectquery = @"
                SELECT MAX(advertisement_id) FROM advertisement;
            ";
            
            string checkquery = @"
                SELECT * 
                FROM advertisement 
                WHERE company_id = @CompanyId AND name = @Name;
            ";


            using var db_connection = dataService.GetConnection();
            
            // Check if the name of the advertisement already exists
            using (var checkcommand = new MySqlCommand(checkquery, db_connection)) {
                checkcommand.Parameters.AddWithValue("@CompanyId", companyId);
                checkcommand.Parameters.AddWithValue("@Name", advertisement.Name);

                using var reader = checkcommand.ExecuteReader();

                if (reader.HasRows) {
                    return -1;
                }
            }

            // Insert the advertisement
            using (var insertcommand = new MySqlCommand(insertquery, db_connection))
            {

                insertcommand.Parameters.AddWithValue("@CompanyId", companyId);
                insertcommand.Parameters.AddWithValue("@Description", advertisement.Description);
                insertcommand.Parameters.AddWithValue("@Name", advertisement.Name);
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

            if (result != null)
            {
                int advId = Convert.ToInt32(result);

                // Add skills if they do not already exist
                List<int> skillIds = AddSkillsIfNotAlreadyPresent(skills);

                // Link skills to the advertisement
                AddSkillsToAdvertisement(advId, skillIds);

                return advId; // Return the advertisement ID
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    private List<int> AddSkillsIfNotAlreadyPresent(List<Entity.Skill> skills)
    {

        var skillIds = new List<int>();

        try
        {
            string insertQuery = @"
            INSERT IGNORE INTO skill (name)
            VALUES (@Name);
        ";
            string selectQuery = @"
            SELECT skill_id FROM skill WHERE name = @Name;
        ";

            using var db_connection = dataService.GetConnection();

            foreach (var skill in skills)
            {
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

    public void MatchAdvertisementForStudent(int advertisementId) {
        try {
            // Query to find students with at least 3 skill matches
            string findStudentsQuery = @"
            SELECT ss.student_id, COUNT(*) AS tot
            FROM student_skills ss
            JOIN advertisement_skills ads ON ss.skill_id = ads.skill_id
            WHERE ads.advertisement_id = @AdvertisementId
            GROUP BY ss.student_id
        ";
            

            // Query to insert notifications
            string insertNotificationQuery = @"
            INSERT INTO student_notifications (student_id, advertisement_id, type)
            VALUES (@StudentId, @AdvertisementId, 'RECOMMENDED');
        ";

            using var db_connection = dataService.GetConnection();

            // Find matching students
            List<int> matchingStudentIds = new List<int>();
            using (var findCommand = new MySqlCommand(findStudentsQuery, db_connection)) {

                findCommand.Parameters.AddWithValue("@AdvertisementId", advertisementId);
                
                using var reader = findCommand.ExecuteReader();

                while (reader.Read()) {
                    if(Convert.ToInt32(reader["tot"]) >= 3)
                        matchingStudentIds.Add(Convert.ToInt32(reader["student_id"]));
                }
            }

            // Insert notifications for matching students
            foreach (var studentId in matchingStudentIds) {
                var insertCommand = new MySqlCommand(insertNotificationQuery, db_connection);
                insertCommand.Parameters.AddWithValue("@StudentId", studentId);
                insertCommand.Parameters.AddWithValue("@AdvertisementId", advertisementId);
                
                insertCommand.ExecuteNonQuery();
                
            }
            
        }
        catch (Exception ex) {
            Console.WriteLine($"Error matching advertisement to students: {ex.Message}");
        }
    }

    public Entity.Advertisement GetAdvertisement(int advertisementId)
    {
        try
        {
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public List<Entity.Student> GetRecommendedCandidates(int companyId, int advertisementId)
    {
        try
        {
            // Query to retrieve student IDs from company_notifications
            string getStudentIdsQuery = @"
                SELECT ss.student_id
                FROM advertisement_skills ads
                INNER JOIN advertisement a ON ads.advertisement_id = a.advertisement_id
                INNER JOIN student_skills ss ON ads.skill_id = ss.skill_id
                WHERE a.advertisement_id = @AdvertisementId
                  AND a.open = true
                GROUP BY a.advertisement_id, ss.student_id
                ORDER BY COUNT(*) DESC
                LIMIT 40;
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
            using (var getIdsCommand = new MySqlCommand(getStudentIdsQuery, db_connection))
            {
                getIdsCommand.Parameters.AddWithValue("@AdvertisementId", advertisementId);

                using var reader = getIdsCommand.ExecuteReader();
                while (reader.Read())
                {
                    studentIds.Add(Convert.ToInt32(reader["student_id"]));
                }
            }

            // Step 2: Retrieve student details
            List<Entity.Student> students = new List<Entity.Student>();
            foreach (var studentId in studentIds)
            {
                using (var getDetailsCommand = new MySqlCommand(getStudentDetailsQuery, db_connection))
                {

                    getDetailsCommand.Parameters.AddWithValue("@StudentId", studentId);

                    using var reader = getDetailsCommand.ExecuteReader();

                    var student = dataService.MapToStudents(reader).FirstOrDefault();

                    students.Add(student);
                }
            }
            
            List<Entity.Student> studentsWithInternships = GetStudentWithInternships(students);
            
            List<Entity.Student> studentsWithOutInternships = students.Except(studentsWithInternships).ToList();
            
            return studentsWithOutInternships;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving recommended candidates: {ex.Message}");
            return new List<Entity.Student>();
        }
    }
    
    private List<Entity.Student> GetStudentWithInternships(List<Entity.Student> students) {
        
        List<Entity.Student> studentsWithInternships = new List<Entity.Student>();
        foreach (var student in students) {
            var internships = GetInternshipForStudent(student.StudentId);
            foreach (var internship in internships) {
                if (internship.EndDate > DateTime.Now) {
                    studentsWithInternships.Add(student);
                    break;
                }
            }

        }
        return studentsWithInternships;
    }
    
    public List<Entity.Internship> GetInternshipForStudent(int studentId) {
        try {
            string query = @"
                SELECT *
                FROM internship
                WHERE student_id = @StudentId
                ORDER BY end_date DESC;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@StudentId", studentId);
            
            using var reader = command.ExecuteReader();
            
            var internships = dataService.MapToInternships(reader);
            
            return internships;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool CreateSuggestionsForStudent(int advertisementId, int studentId, int companyId)
    {
        try
        {
            // Query to insert the notification into student_notifications
            string insertNotificationQuery = @"
                INSERT INTO student_notifications (student_id, advertisement_id, type)
                VALUES (@StudentId, @AdvertisementId, 'INVITED');
            ";
            
            // Query to check if the notification already exists
            string checkNotificationQuery = @"
                SELECT COUNT(*) 
                FROM student_notifications 
                WHERE student_id = @StudentId 
                  AND advertisement_id = @AdvertisementId 
                  AND type = 'INVITED';
            ";
            
            
            using var db_connection = dataService.GetConnection();
            
            
            // Check if the notification already exists
            using (var checkCommand = new MySqlCommand(checkNotificationQuery, db_connection)) {
                checkCommand.Parameters.AddWithValue("@StudentId", studentId);
                checkCommand.Parameters.AddWithValue("@AdvertisementId", advertisementId);

                var result = checkCommand.ExecuteScalar();
                var existingCount = (result != null) ? Convert.ToInt32(result) : 0;

                // If the notification already exists, return false
                if (existingCount > 0) 
                    return false;
            }

            
            // Check if company is the owner of the advertisement
            var advertisement = GetAdvertisement(advertisementId);
            if (advertisement == null || advertisement.CompanyId != companyId) {
                return false;
            }

            // Insert the notification into student_notifications
            using (var insertCommand = new MySqlCommand(insertNotificationQuery, db_connection))
            {

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

    public bool DeleteAdvertisement(int advertisementId, int companyId)
    {
        try
        {
            string query = @"
                DELETE FROM advertisement
                WHERE advertisement_id = @AdvertisementId
            ";

            // Check if company is the owner of the advertisement
            var advertisement = GetAdvertisement(advertisementId);
            if (advertisement == null || advertisement.CompanyId != companyId)
            {
                return false;
            }

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@AdvertisementId", advertisementId);
            command.Parameters.AddWithValue("@CompanyId", companyId);

            command.ExecuteNonQuery();

            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}

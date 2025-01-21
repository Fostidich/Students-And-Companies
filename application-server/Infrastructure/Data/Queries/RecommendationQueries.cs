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
    
    public bool CreateAdvertisement(int companyId, Entity.Advertisement advertisement) {
        try {
            string query = @"
                INSERT INTO advertisement (company_id, description, duration, spots, available, open, questionnaire)
                VALUES (@CompanyId, @Description, @Duration, @Spots, @Avaible, @Open, @Questionnaire);
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
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    

}

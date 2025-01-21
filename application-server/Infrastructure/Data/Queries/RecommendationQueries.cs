using System;
using System.Linq;
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
                FROM Advertisements a
                WHERE a.CompanyId = @CompanyId;
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
                    a.Id AS AdvertisementId,
                    a.CreatedAt,
                    a.CompanyId,
                    a.Description,
                    a.Duration,
                    a.Spots,
                    a.Available,                   
                    a.Open,
                    a.Questionnaire,
                FROM AdvertisementSkills ads
                INNER JOIN Advertisement a ON ads.AdvertisementId = a.Id
                INNER JOIN StudentSkills ss ON ads.SkillId = ss.SkillId
                WHERE ss.StudentId = @StudentId
                  AND a.Open = TRUE -- Consider only open advertisements
                GROUP BY a.Id, a.CreatedAt, a.CompanyId, a.Description, a.Duration, a.Spots, a.Available, a.Open, a.Questionnaire 
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
                INSERT INTO Advertisement (company_id, description, duration, spots, available, open, questionnaire)
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

using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

public class InternshipQueries : IInternshipQueries {
    
    private readonly IDataService dataService;
    
    public InternshipQueries(IDataService dataService) {
        this.dataService = dataService;
    }
    
    public Entity.Internship GetInternshipForStudent(int studentId) {
        try {
            string query = @"
                SELECT *
                FROM internship
                WHERE student_id = @StudentId;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@StudentId", studentId);
            
            using var reader = command.ExecuteReader();
            
            var internship = dataService.MapToInternships(reader).FirstOrDefault();
            return internship;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    public List<Entity.Internship> GetInternshipFromAdvertisement(int advertisementId) {
        try {
            string query = @"
                SELECT *
                FROM internship
                WHERE advertisement_id = @AdvertisementId;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@AdvertisementId", advertisementId);
            
            using var reader = command.ExecuteReader();
            
            var internships = dataService.MapToInternships(reader);
            return internships;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    public bool CreateStudentFeedback(int internshipId, Entity.StudentFeedback feedback) {
        try {
            string query = @"
                INSERT INTO student_feedback (internship_id, rating, comment)
                VALUES (@InternshipId, @Rating, @Comment);
            ";
            
            // Check if internship is finished
            bool checkFinished = CheckIfInternshipFinished(internshipId);
            if (!checkFinished) {
                return false;
            }
            
            // Check if student feedback already exists
            var studentFeedback = GetStudentFeedback(internshipId);
            if (studentFeedback != null) {
                return false;
            }
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@InternshipId", internshipId);
            command.Parameters.AddWithValue("@Rating", feedback.Rating);
            command.Parameters.AddWithValue("@Comment", feedback.Comment);
            
            command.ExecuteNonQuery();
            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    
    public bool CreateCompanyFeedback(int internshipId, Entity.CompanyFeedback feedback) {
        try {
            string query = @"
                INSERT INTO company_feedback (internship_id, rating, comment)
                VALUES (@InternshipId, @Rating, @Comment);
            ";
            
            // Check if internship is finished
            bool checkFinished = CheckIfInternshipFinished(internshipId);
            if (!checkFinished) {
                return false;
            }
            
            // Check if company feedback already exists
            var companyFeedback = GetCompanyFeedback(internshipId);
            if (companyFeedback != null) {
                return false;
            }
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@InternshipId", internshipId);
            command.Parameters.AddWithValue("@Rating", feedback.Rating);
            command.Parameters.AddWithValue("@Comment", feedback.Comment);
            
            command.ExecuteNonQuery();
            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    
    public Entity.StudentFeedback GetStudentFeedback(int internshipId) {
        try {
            string query = @"
                SELECT *
                FROM student_feedback
                WHERE internship_id = @InternshipId;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@InternshipId", internshipId);
            
            using var reader = command.ExecuteReader();
            
            Entity.StudentFeedback feedback = dataService.MapToStudentFeedback(reader).FirstOrDefault();
            return feedback;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    public Entity.CompanyFeedback GetCompanyFeedback(int internshipId) {
        try {
            string query = @"
                SELECT *
                FROM company_feedback
                WHERE internship_id = @InternshipId;
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@InternshipId", internshipId);
            
            using var reader = command.ExecuteReader();
            
            Entity.CompanyFeedback feedback = dataService.MapToCompanyFeedback(reader).FirstOrDefault();
            return feedback;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    private bool CheckIfInternshipFinished(int internshipId) {
        try {
            string query = @"
                SELECT *
                FROM internship
                WHERE internship_id = @InternshipId AND end_date < NOW();
            ";
            
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@InternshipId", internshipId);
            
            using var reader = command.ExecuteReader();
            
            if (reader.HasRows) {
                return true;
            }
            
            return false;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}

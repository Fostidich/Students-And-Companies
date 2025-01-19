using System;
using System.Data;
using MySql.Data.MySqlClient; 
using System.Linq; 


public class ProfileQueries : IProfileQueries {

    private readonly IDataService dataService;

    public ProfileQueries(IDataService dataService) {
        this.dataService = dataService;
    }

    public Entity.Company FindCompanyFromId(int id) {
        try {
            string query = @"
                SELECT *
                FROM Company
                WHERE CompanyId = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@Id", id);
            
            using var reader = command.ExecuteReader();
            
            var companies = dataService.MapToCompany(reader);
            return companies.FirstOrDefault();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    public Entity.Student FindStudentFromId(int id) {
        try {
            string query = @"
                SELECT *
                FROM Student
                WHERE StudentId = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@Id", id);
            
            using var reader = command.ExecuteReader();
            
            var students = dataService.MapToStudent(reader);
            return students.FirstOrDefault();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    public bool UpdateSaltAndPassword(UserType type, int id, string salt, string hash) {
        try {
            string tableName = type == UserType.Company ? "Companies" : "Students";
            string idColumn = type == UserType.Company ? "CompanyId" : "StudentId";
            string query = $@"
                UPDATE {tableName}
                SET Salt = @Salt, HashedPassword = @HashedPassword
                WHERE {idColumn} = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@Salt", salt);
            command.Parameters.AddWithValue("@HashedPassword", hash);
            command.Parameters.AddWithValue("@Id", id);
            
            int rowsAffected = command.ExecuteNonQuery();
            
            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    
    public bool UpdateUsername(UserType type, int id, string username) {
        try {
            string tableName = type == UserType.Company ? "Companies" : "Students";
            string idColumn = type == UserType.Company ? "CompanyId" : "StudentId";
            string query = $@"
                UPDATE {tableName}
                SET Username = @Username
                WHERE {idColumn} = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Id", id);
            
            int rowsAffected = command.ExecuteNonQuery();
            
            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public bool UpdateEmail(UserType type, int id, string email) {
        try {
            string tableName = type == UserType.Company ? "Companies" : "Students";
            string idColumn = type == UserType.Company ? "CompanyId" : "StudentId";
            string query = $@"
                UPDATE {tableName}
                SET Email = @Email
                WHERE {idColumn} = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);
            
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Id", id);
            
            int rowsAffected = command.ExecuteNonQuery();
            
            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}

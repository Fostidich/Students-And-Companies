using System;
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
                FROM company
                WHERE company_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            var companies = dataService.MapToCompanies(reader);
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
                FROM student
                WHERE student_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            var students = dataService.MapToStudents(reader);
            return students.FirstOrDefault();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool UpdateSaltAndPassword(UserType type, int id, string salt, string hash) {
        try {
            string tableName = type == UserType.Company ? "company" : "student";
            string idColumn = type == UserType.Company ? "company_id" : "student_id";
            string query = $@"
                UPDATE {tableName}
                SET salt = @Salt, hashed_password = @HashedPassword
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
            string tableName = type == UserType.Company ? "company" : "student";
            string idColumn = type == UserType.Company ? "company_id" : "student_id";
            string query = $@"
                UPDATE {tableName}
                SET username = @Username
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
            string tableName = type == UserType.Company ? "company" : "student";
            string idColumn = type == UserType.Company ? "company_id" : "student_id";
            string query = $@"
                UPDATE {tableName}
                SET email = @Email
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

    // TODO
    public bool UpdateBio(UserType type, int id, string bio) {return false;}
    public bool UpdateHeadquarter(int id, string headquaters) {return false;}
    public bool UpdateFiscalCode(int id, string fiscalCode) {return false;}
    public bool UpdateVatNumber(int id, string vatNumber) {return false;}
    public bool UpdateName(int id, string name) {return false;}
    public bool UpdateSurname(int id, string surname) {return false;}
    public bool UpdateUniversity(int id, string university) {return false;}
    public bool UpdateCourseOfStudy(int id, string courseOfStudy) {return false;}
    public bool UpdateGender(int id, char gender) {return false;}
    public bool UpdateBirthDate(int id, DateTime birthDate) {return false;}

}

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

    public bool UpdateBio(UserType type, int id, string bio) {
        try {
            string tableName = type == UserType.Company ? "company" : "student";
            string idColumn = type == UserType.Company ? "company_id" : "student_id";
            string query = $@"
                UPDATE {tableName}
                SET bio = @Bio
                WHERE {idColumn} = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Bio", bio);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateHeadquarter(int id, string headquater) {
        try {
            string query = $@"
                UPDATE company
                SET headquater = @Headquater
                WHERE company_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Headquater", headquater);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateFiscalCode(int id, string fiscalCode) {
        try {
            string query = $@"
                UPDATE company
                SET fiscal_code = @FiscalCode
                WHERE company_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@fiscal_code", fiscalCode);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateVatNumber(int id, string vatNumber) {
        try {
            string query = $@"
                UPDATE company
                SET vat_number = @VatNumber
                WHERE company_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@VatNumber", vatNumber);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateName(int id, string name) {
        try {
            string query = $@"
                UPDATE student
                SET name = @Name
                WHERE student_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateSurname(int id, string surname) {
        try {
            string query = $@"
                UPDATE student
                SET surname = @Surname
                WHERE student_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Surname", surname);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateUniversity(int id, string university) {
        try {
            string query = $@"
                UPDATE student
                SET university = @University
                WHERE student_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@University", university);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateCourseOfStudy(int id, string courseOfStudy) {
        try {
            string query = $@"
                UPDATE student
                SET course_of_study = @CourseOfStudy
                WHERE student_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@CourseOfStudy", courseOfStudy);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateGender(int id, char gender) {
        try {
            string query = $@"
                UPDATE student
                SET gender = @Gender
                WHERE student_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.Add(new MySqlParameter("@Gender", MySqlDbType.VarChar) { Value = gender, Size = 1 });
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateBirthDate(int id, DateTime birthDate) {
        try {
            string query = $@"
                UPDATE student
                SET birth_date = @BirthDate
                WHERE student_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@BirthDate", birthDate);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}

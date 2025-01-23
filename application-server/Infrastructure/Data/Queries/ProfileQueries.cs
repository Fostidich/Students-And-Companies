using System;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

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

    public bool UpdateHeadquarter(int id, string headquarter) {
        try {
            string query = $@"
                UPDATE company
                SET headquarter = @Headquarter
                WHERE company_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Headquarter", headquarter);
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

            command.Parameters.AddWithValue("@FiscalCode", fiscalCode);
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

    public bool DeleteUser(UserType type, int id) {
        try {
            string tableName = type == UserType.Company ? "company" : "student";
            string idColumn = type == UserType.Company ? "company_id" : "student_id";
            string query = $@"
                DELETE FROM {tableName}
                WHERE {idColumn} = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public int FindSkill(string name) {
        try {
            string query = $@"
                SELECT skill_id
                FROM skill
                WHERE name = @Name";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Name", name);

            using var reader = command.ExecuteReader();

            var ids = dataService.MapToInts(reader, "skill_id");
            return ids.FirstOrDefault();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    public bool AddSkill(string name) {
        try {
            string query = $@"
                INSERT INTO skill (name)
                VALUES (@Name)";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Name", name);

	        int rowsAffected = command.ExecuteNonQuery();

	        return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool AddSkillToStudent(int studentId, int skillId) {
        try {
            string query = $@"
                INSERT IGNORE INTO student_skills (student_id, skill_id)
                VALUES (@StudentId, @SkillId)";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@StudentId", studentId);
            command.Parameters.AddWithValue("@SkillId", skillId);

	        command.ExecuteNonQuery();

	        return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public List<Entity.Skill> GetSkills(int id) {
        try {
            string query = $@"
                SELECT s.skill_id, name
                FROM skill AS s
                JOIN student_skills AS ss
                ON s.skill_id = ss.skill_id
                WHERE student_id = @Id";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            var skills = dataService.MapToSkills(reader);
            return skills;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool DeleteSkill(int skillId, int studentId) {
        try {
            string query = $@"
                DELETE FROM student_skills
                WHERE skill_id = @SkillId
                AND student_id = @StudentId";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@SkillId", skillId);
            command.Parameters.AddWithValue("@StudentId", studentId);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}

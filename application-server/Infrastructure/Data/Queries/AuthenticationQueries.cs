using System;
using System.Data;
using MySql.Data.MySqlClient; 
using System.Linq; 


public class AuthenticationQueries : IAuthenticationQueries {

    private readonly IDataService dataService;

    public AuthenticationQueries(IDataService dataService) {
        this.dataService = dataService;
    }

    public bool RegisterCompany(Entity.Company user) {
	    try {
    	    string query = @"
                INSERT INTO Company (Username, Email, Salt, HashedPassword, Bio, Headquarters, FiscalCode, VatNumber)
                VALUES (@Username, @Email, @Salt, @HashedPassword, @Bio, @Headquarters, @FiscalCode, @VatNumber)";
	
    	    using var db_connection = dataService.GetConnection();
        	using var command = new MySqlCommand(query, db_connection);

	        command.Parameters.AddWithValue("@Username", user.Username);
    	    command.Parameters.AddWithValue("@Email", user.Email);
        	command.Parameters.AddWithValue("@Salt", user.Salt);
        	command.Parameters.AddWithValue("@HashedPassword", user.HashedPassword);
        	command.Parameters.AddWithValue("@Bio", user.Bio ?? (object)DBNull.Value);
        	command.Parameters.AddWithValue("@Headquarters", user.Headquarters);
        	command.Parameters.AddWithValue("@FiscalCode", user.FiscalCode);
        	command.Parameters.AddWithValue("@VatNumber", user.VatNumber);

	        int rowsAffected = command.ExecuteNonQuery();

	        return rowsAffected > 0;
    	} catch (Exception ex) {
        	Console.WriteLine(ex.Message);
        	return false;
    	}
	}

	public bool RegisterStudent(Entity.Student user) {
	    try {
	        string query = @"
                INSERT INTO Student (Username, Email, Salt, HashedPassword, Bio, Name, Surname, University, StudyCourse, Sex, BirthDate)
                VALUES (@Username, @Email, @Salt, @HashedPassword, @Bio, @Name, @Surname, @University, @StudyCourse, @Sex, @BirthDate)";

	        using var db_connection = dataService.GetConnection();
	        using var command = new MySqlCommand(query, db_connection);

	        command.Parameters.AddWithValue("@Username", user.Username);
	        command.Parameters.AddWithValue("@Email", user.Email);
	        command.Parameters.AddWithValue("@Salt", user.Salt);
	        command.Parameters.AddWithValue("@HashedPassword", user.HashedPassword);
	        command.Parameters.AddWithValue("@Bio", user.Bio ?? (object)DBNull.Value);
	        command.Parameters.AddWithValue("@Name", user.Name);
	        command.Parameters.AddWithValue("@Surname", user.Surname);
	        command.Parameters.AddWithValue("@University", user.University);
	        command.Parameters.AddWithValue("@StudyCourse", user.StudyCourse);
	        command.Parameters.AddWithValue("@Sex", user.Sex);
	        command.Parameters.AddWithValue("@BirthDate", user.BirthDate);

	        int rowsAffected = command.ExecuteNonQuery();

	        return rowsAffected > 0;
	    } catch (Exception ex) {
	        Console.WriteLine(ex.Message);
	        return false;
	    }
	}

	public Entity.Company FindCompanyFromUsername(string username) {
	    try {
	        string query = @"
                SELECT *
                FROM Company
                WHERE LOWER(username) = LOWER(@Username)";

	        using var db_connection = dataService.GetConnection();
	        using var command = new MySqlCommand(query, db_connection);
	        
			command.Parameters.AddWithValue("@Username", username);

			using var reader = command.ExecuteReader();

	        var companies = dataService.MapToCompany(reader);
	        return companies.FirstOrDefault();
	    } catch (Exception ex) {
	        Console.WriteLine(ex.Message);
	        return null;
	    }
	}
	
	public Entity.Student FindStudentFromUsername(string username) {
	    try {
	        string query = @"
                SELECT *
                FROM Student
                WHERE LOWER(username) = LOWER(@Username)";

	        using var db_connection = dataService.GetConnection();
	        using var command = new MySqlCommand(query, db_connection);

	        command.Parameters.AddWithValue("@Username", username);

	        using var reader = command.ExecuteReader();

	        var students = dataService.MapToStudent(reader);
	        return students.FirstOrDefault();
	    } catch (Exception ex) {
	        Console.WriteLine(ex.Message);
        	return null;
	    }
	}

	public Entity.Company FindCompanyFromEmail(string email) {
	    try {
	        string query = @"
                SELECT *
                FROM Company
                WHERE LOWER(email) = LOWER(@Email)";

	        using var db_connection = dataService.GetConnection();
	        using var command = new MySqlCommand(query, db_connection);

	        command.Parameters.AddWithValue("@Email", email);

	        using var reader = command.ExecuteReader();

	        var companies = dataService.MapToCompany(reader);
	        return companies.FirstOrDefault();
	    } catch (Exception ex) {
	        Console.WriteLine(ex.Message);
	        return null;
	    }
	}
	
	public Entity.Student FindStudentFromEmail(string email) {
	    try {
	        string query = @"
                SELECT *
                FROM Student
                WHERE LOWER(email) = LOWER(@Email)";

	        using var db_connection = dataService.GetConnection();
	        using var command = new MySqlCommand(query, db_connection);

	        command.Parameters.AddWithValue("@Email", email);

	        using var reader = command.ExecuteReader();

	        var students = dataService.MapToStudent(reader);
	        return students.FirstOrDefault();
	    } catch (Exception ex) {
	        Console.WriteLine(ex.Message);
	        return null;
	    }
	}
}

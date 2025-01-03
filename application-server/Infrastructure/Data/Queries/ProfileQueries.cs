using System;
using MySql.Data.MySqlClient;

public class ProfileQueries : IProfileQueries {

    private readonly IDataService dataService;

    public ProfileQueries(IDataService dataService) {
        this.dataService = dataService;
    }

    public Entity.User FindFromUserId(int id) {
        try {
            string query = @"
                SELECT *
                FROM users
                WHERE id = @id";
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            var users = dataService.MapToUsers(reader);
            if (users.Count == 0) return null;
            return users[0];
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool UpdateSaltAndPassword(int id, string salt, string hash) {
        try {
            string query = @"
                UPDATE users
                SET salt = @salt, hashed_password = @hashed_password
                WHERE id = @id";
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@salt", salt);
            command.Parameters.AddWithValue("@hashed_password", hash);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateUsername(int id, string username) {
        try {
            string query = @"
                UPDATE users
                SET username = @username
                WHERE id = @id";
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateEmail(int id, string email) {
        try {
            string query = @"
                UPDATE users
                SET email = @email
                WHERE id = @id";
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}

using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;

public class AuthenticationQueries : IAuthenticationQueries {

    private readonly IDataService dataService;

    public AuthenticationQueries(IDataService dataService) {
        this.dataService = dataService;
    }

    public bool RegisterUser(Entity.User user) {
        try {
            string query = @"
                INSERT INTO users (username, email, salt, hashed_password, user_type)
                VALUES (@username, @email, @salt, @hashed_password, @user_type)";
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@salt", user.Salt);
            command.Parameters.AddWithValue("@hashed_password", user.HashedPassword);
            command.Parameters.AddWithValue("@user_type", user.UserType);
            command.ExecuteNonQuery();

            return true;
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public Entity.User FindFromUsername(string username) {
        try {
            string query = @"
                SELECT *
                FROM users
                WHERE LOWER(username) = LOWER(@username)";
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@username", username);
            using var reader = command.ExecuteReader();

            var users = MapToUsers(reader);
            if (users.Count == 0) return null;
            return users[0];
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public Entity.User FindFromEmail(string email) {
        try {
            string query = @"
                SELECT *
                FROM users
                WHERE email = @email";
            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@email", email);
            using var reader = command.ExecuteReader();

            var users = MapToUsers(reader);
            if (users.Count == 0) return null;
            return users[0];
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    private List<Entity.User> MapToUsers(IDataReader reader) {
        var users = new List<Entity.User>();

        while (reader.Read()) {
            var user = new Entity.User {
                Id = Convert.ToInt32(reader["id"]),
                Username = reader["username"].ToString(),
                Email = reader["email"].ToString(),
                Salt = reader["salt"].ToString(),
                HashedPassword = reader["hashed_password"].ToString(),
                UserType = reader["user_type"].ToString(),
                CreatedAt = reader["created_at"].ToString()
            };
            users.Add(user);
        }

        return users;
    }

}

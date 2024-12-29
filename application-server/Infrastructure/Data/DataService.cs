using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

public static class DataService {

    public static MySqlConnection GetConnection() {
        string default_connection = Environment.GetEnvironmentVariable("DB_DEFAULT_CONNECTION");
        var connection = new MySqlConnection(default_connection);
        connection.Open();
        return connection;
    }

    public static List<Entity.User> MapToUsers(IDataReader reader) {
        var users = new List<Entity.User>();

        while (reader.Read()) {
            var user = new Entity.User {
                Id = Convert.ToInt32(reader["id"]),
                Username = reader["username"].ToString(),
                Email = reader["email"].ToString(),
                HashedPassword = reader["hashed_password"].ToString(),
                UserType = reader["user_type"].ToString(),
                CreatedAt = reader["created_at"].ToString()
            };
            users.Add(user);
        }

        return users;
    }

}


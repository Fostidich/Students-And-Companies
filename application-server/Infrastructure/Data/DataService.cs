using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

public class DataService : IDataService {

    private string defaultConnection;

    public DataService(IConfiguration configuration) {
        defaultConnection = configuration["DbDefaultConnection"];
    }

    public MySqlConnection GetConnection() {
        var connection = new MySqlConnection(defaultConnection);
        connection.Open();
        return connection;
    }

    public List<Entity.User> MapToUsers(IDataReader reader) {
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


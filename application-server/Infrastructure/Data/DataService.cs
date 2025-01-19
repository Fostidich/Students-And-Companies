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

    public List<Entity.Student> MapToStudents(IDataReader reader) {
        var users = new List<Entity.Student>();

        while (reader.Read()) {
            var user = new Entity.Student {
                StudentId = Convert.ToInt32(reader["student_id"]),
                CreatedAt = DateTime.Parse(reader["created_at"].ToString()),
                Email = reader["email"].ToString(),
                Username = reader["username"].ToString(),
                Salt = reader["salt"].ToString(),
                HashedPassword = reader["hashed_password"].ToString(),
                Bio = reader["bio"].ToString(),
                Name = reader["name"].ToString(),
                Surname = reader["surname"].ToString(),
                University = reader["university"].ToString(),
                CourseOfStudy = reader["course_of_study"].ToString(),
                Gender = reader["gender"].ToString()[0],
                BirthDate = DateTime.Parse(reader["birth_date"].ToString()),
            };
            users.Add(user);
        }

        return users;
    }

    public List<string> MapToStrings(IDataReader reader, string fieldName) {
        var result = new List<string>();

        while (reader.Read()) {
            result.Add(reader[fieldName].ToString());
        }

        return result;
    }

}


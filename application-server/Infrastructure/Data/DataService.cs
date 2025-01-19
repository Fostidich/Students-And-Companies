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

    // Methods to map data from IDataReader 
    
    public List<Entity.Student> MapToStudent(IDataReader reader) {
        var students = new List<Entity.Student>();

        while (reader.Read()) {
            students.Add(new Entity.Student(reader));
        }

        return students;
    }
    
    public List<Entity.Company> MapToCompany(IDataReader reader) {
        var companies = new List<Entity.Company>();

        while (reader.Read()) {
            companies.Add(new Entity.Company(reader));
        }

        return companies;
    }

    public List<string> MapToStrings(IDataReader reader, string fieldName) {
        var result = new List<string>();

        while (reader.Read()) {
            result.Add(reader[fieldName].ToString());
        }

        return result;
    }

}


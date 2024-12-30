using System;
using MySql.Data.MySqlClient;

public static class DataService {

    private static string defaultConnection;

    static DataService() {
        defaultConnection = "Server=localhost;Database=students_and_companies;User ID=sc_admin;Password=;";
    }

    public static bool LoadDefaultConnection() {
        string connectionString = Environment.GetEnvironmentVariable("DB_DEFAULT_CONNECTION");
        if (string.IsNullOrEmpty(connectionString)) {
            Console.WriteLine($"No connection string found in .env: using \"{defaultConnection}\".");
            return false;
        }
        else
            defaultConnection = connectionString;
            return true;
    }

    public static MySqlConnection GetConnection() {
        var connection = new MySqlConnection(defaultConnection);
        connection.Open();
        return connection;
    }

}


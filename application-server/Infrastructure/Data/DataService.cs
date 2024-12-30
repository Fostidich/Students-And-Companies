using System;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

public class DataService {

    private static string defaultConnection;

    public DataService(IConfiguration configuration) {
        // FIXME no calls to dotenv here id say
        defaultConnection = configuration["DbDefaultConnection"];
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


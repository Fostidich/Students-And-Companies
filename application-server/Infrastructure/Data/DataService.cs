using System;
using MySql.Data.MySqlClient;

public static class DataService {

    public static MySqlConnection GetConnection() {
        string default_connection = Environment.GetEnvironmentVariable("DB_DEFAULT_CONNECTION");
        var connection = new MySqlConnection(default_connection);
        connection.Open();
        return connection;
    }

}


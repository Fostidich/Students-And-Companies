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

}


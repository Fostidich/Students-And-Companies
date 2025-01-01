using MySql.Data.MySqlClient;

public interface IDataService {

    MySqlConnection GetConnection();

}

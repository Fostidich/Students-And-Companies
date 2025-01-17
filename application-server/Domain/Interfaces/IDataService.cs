using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public interface IDataService {

    MySqlConnection GetConnection();
    List<Entity.User> MapToUsers(IDataReader reader);
    List<string> MapToStrings(IDataReader reader, string fieldName);

}

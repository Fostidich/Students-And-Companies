using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public interface IDataService {

    MySqlConnection GetConnection();
    List<Entity.Student> MapToStudents(IDataReader reader);
    List<Entity.Company> MapToCompanies(IDataReader reader);
    List<Entity.Skill> MapToSkills(IDataReader reader);
    List<Entity.Advertisement> MapToAdvertisements(IDataReader reader);
    List<string> MapToStrings(IDataReader reader, string fieldName);
    List<int> MapToInts(IDataReader reader, string fieldName);

}

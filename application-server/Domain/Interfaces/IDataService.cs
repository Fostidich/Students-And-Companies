using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public interface IDataService {

    MySqlConnection GetConnection();
    List<Entity.Student> MapToStudents(IDataReader reader);
    List<Entity.Company> MapToCompanies(IDataReader reader);
    List<Entity.Skill> MapToSkills(IDataReader reader);
    List<Entity.Advertisement> MapToAdvertisements(IDataReader reader);
    List<Entity.StudentNotifications> MapToStudentNotifications(IDataReader reader);
    List<Entity.Application> MapToApplications(IDataReader reader);
    List<Entity.Internship> MapToInternships(IDataReader reader);
    List<Entity.StudentFeedback> MapToStudentFeedback(IDataReader reader);
    List<Entity.CompanyFeedback> MapToCompanyFeedback(IDataReader reader);
    List<string> MapToStrings(IDataReader reader, string fieldName);
    List<int> MapToInts(IDataReader reader, string fieldName);

}

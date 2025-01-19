public interface IProfileQueries {

    Entity.Company FindCompanyFromId(int id);
    Entity.Student FindStudentFromId(int id);
    bool UpdateSaltAndPassword(UserType type, int id, string salt, string hash);
    bool UpdateUsername(UserType type, int id, string username);
    bool UpdateEmail(UserType type, int id, string email);
    bool SetCvFilePath(int id, string filePath);
    string GetCvFilePath(int id);
    bool RemoveCvFilePath(int id);

}

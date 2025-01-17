public interface IProfileQueries {

    Entity.User FindFromUserId(int id);
    bool UpdateSaltAndPassword(int id, string salt, string hash);
    bool UpdateUsername(int id, string username);
    bool UpdateEmail(int id, string email);
    bool SetCvFilePath(int id, string filePath);
    string GetCvFilePath(int id);
    bool RemoveCvFilePath(int id);

}

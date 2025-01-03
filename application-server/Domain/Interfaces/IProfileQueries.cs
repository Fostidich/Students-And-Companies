public interface IProfileQueries {

    Entity.User FindFromUserId(int id);
    bool UpdateSaltAndPassword(int id, string salt, string hash);
    bool UpdateUsername(int id, string username);
    bool UpdateEmail(int id, string email);

}

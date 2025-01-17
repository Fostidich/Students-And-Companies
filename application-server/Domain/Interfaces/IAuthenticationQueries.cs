public interface IAuthenticationQueries {

    bool RegisterUser(Entity.User user);
    Entity.User FindFromUsername(string username);
    Entity.User FindFromEmail(string email);

}


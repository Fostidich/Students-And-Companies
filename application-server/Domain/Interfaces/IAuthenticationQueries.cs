public interface IAuthenticationQueries {

    bool RegisterUser(Entity.User user);
    public Entity.User FindFromUsername(string username);
    public Entity.User FindFromEmail(string email);

}


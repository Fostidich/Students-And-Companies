public interface IAuthenticationQueries {

    bool RegisterUser(Entity.User user);
    Entity.User GetUser(int id);

}


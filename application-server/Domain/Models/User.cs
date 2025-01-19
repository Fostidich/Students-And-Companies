public class User {

    public int Id { get; }
    public string Username { get; }
    public string Salt { get; }
    public string HashedPassword { get; }
    public UserType UserType { get; }
    public string Email { get; }

}

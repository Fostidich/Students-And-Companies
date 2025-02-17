using System;

public abstract class User
{

    public int Id { get; set; }
    public string Username { get; set; }
    public string Salt { get; set; }
    public string HashedPassword { get; set; }
    public UserType UserType { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }

}

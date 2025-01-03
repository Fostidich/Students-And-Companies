using System;

public class User {

    public int Id { get; }
    public string Username { get; }
    public UserType UserType { get; }
    public string Email { get; }

    public User(Entity.User user) {
        Id = user.Id;
        Username = user.Username;
        UserType = UserTypeFromString(user.UserType);
        Email = user.Email;
    }

    public User(DTO.User user) {
        Id = user.Id;
        Username = user.Username;
        UserType = UserTypeFromString(user.UserType);
        Email = user.Email;
    }

    public Entity.User ToEntity() {
        return new Entity.User {
            Id = Id,
            Username = Username,
            UserType = UserType.ToString(),
            Email = Email
        };
    }

    public DTO.User ToDto() {
        return new DTO.User {
            Id = Id,
            Username = Username,
            UserType = UserType.ToString(),
            Email = Email
        };
    }

    public static UserType UserTypeFromString(string x) {
        if (x.Equals("Company")) return UserType.Company;
        if (x.Equals("Student")) return UserType.Student;
        throw new ArgumentException($"Invalid UserType: {x}");
    }

}

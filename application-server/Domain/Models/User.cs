using System;

public class User {

    private int id { get; }
    private string username { get; }
    private UserType userType { get; }
    private string email { get; }

    public User(Entity.User user) {
        id = user.Id;
        username = user.Username;
        userType = UserTypeFromString(user.UserType);
        email = user.Email;
    }

    public User(DTO.User user) {
        id = user.Id;
        username = user.Username;
        userType = UserTypeFromString(user.UserType);
        email = user.Email;
    }

    public Entity.User ToEntity() {
        return new Entity.User {
            Id = id,
            Username = username,
            UserType = userType.ToString(),
            Email = email
        };
    }

    public DTO.User ToDto() {
        return new DTO.User {
            Id = id,
            Username = username,
            UserType = userType.ToString(),
            Email = email
        };
    }

    public static UserType UserTypeFromString(string x) {
        if (x.Equals("Company")) return UserType.Company;
        if (x.Equals("Student")) return UserType.Student;
        throw new ArgumentException($"Invalid UserType: {x}");
    }

}

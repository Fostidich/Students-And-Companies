public class AuthenticationService : IAuthenticationService {

    private readonly IAuthenticationQueries queries;

    public AuthenticationService(IAuthenticationQueries queries) {
        this.queries = queries;
    }

    public bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm) {
        // TODO check that username and email are unique
        return true;
    }

    public bool RegisterUser(DTO.RegistrationForm registrationForm) {
        var user = new Entity.User {
            Username = registrationForm.Username,
            Email = registrationForm.Email,
            HashedPassword = registrationForm.Password, // TODO hash and salt the password
            UserType = registrationForm.UserType
        };
        return queries.RegisterUser(user);
    }

    public DTO.User GetUser(int id) {
        // TODO check if query goes well
        var userEntity = queries.GetUser(id);
        var userDto = new DTO.User {
            Id = userEntity.Id,
            Username = userEntity.Username,
            UserType = userEntity.UserType,
            Email = userEntity.Email
        };
        return userDto;
    }

}

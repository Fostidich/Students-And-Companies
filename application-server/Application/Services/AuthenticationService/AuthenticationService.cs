public static class AuthenticationService {

    public static bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm) {
        // TODO check that username and email are unique
        return true;
    }

    public static bool RegisterUser(DTO.RegistrationForm registrationForm) {
        var user = new Entity.User {
            Username = registrationForm.Username,
            Email = registrationForm.Email,
            HashedPassword = registrationForm.Password, // TODO hash and salt the password
            UserType = registrationForm.UserType
        };
        return AuthenticationQueries.RegisterUser(user);
    }

    public static DTO.User GetUser(int id) {
        // TODO check if query goes well
        var userEntity = AuthenticationQueries.GetUser(id);
        var userDto = new DTO.User {
            Id = userEntity.Id,
            Username = userEntity.Username,
            UserType = userEntity.UserType,
            Email = userEntity.Email
        };
        return userDto;
    }

}

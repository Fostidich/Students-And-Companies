public static class AuthenticationService {

    public static bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm) {
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

}

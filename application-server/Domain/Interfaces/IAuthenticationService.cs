public interface IAuthenticationService {

    bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm);
    bool RegisterUser(DTO.RegistrationForm registrationForm);
    User ValidateCredentials(DTO.Credentials credentials);
    string GenerateToken(User user);
    string HashPassword(string salt, string password);
    string GenerateSalt();
    bool IsValidEmail(string email);

}

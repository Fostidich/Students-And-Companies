public interface IAuthenticationService {

    bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm);
    bool RegisterUser(DTO.RegistrationForm registrationForm);
    DTO.User ValidateCredentials(DTO.Credentials credentials);
    string GenerateToken(DTO.User user);
    string HashPassword(string salt, string password);
    string GenerateSalt();
    bool IsValidEmail(string email);

}

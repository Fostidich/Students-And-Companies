public interface IAuthenticationService {

    bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm);
    bool RegisterUser(DTO.RegistrationForm registrationForm);
    public DTO.User ValidateCredentials(DTO.Credentials credentials);
    string GenerateToken(DTO.User user);

}

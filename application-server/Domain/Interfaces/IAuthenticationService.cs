public interface IAuthenticationService {

    bool IsCompanyRegistrationValid(DTO.RegistrationFormCompany registrationForm);
    bool IsStudentRegistrationValid(DTO.RegistrationFormStudent registrationForm);
    bool RegisterCompany(DTO.RegistrationFormCompany registrationForm);
    bool RegisterStudent(DTO.RegistrationFormStudent registrationForm);
    User ValidateCredentials(DTO.Credentials credentials);
    string GenerateToken(User user);
    string HashPassword(string salt, string password);
    string GenerateSalt();
    bool IsValidEmail(string email);

}

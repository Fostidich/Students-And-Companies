public interface IAuthenticationService {

    bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm);
    bool RegisterUser(DTO.RegistrationForm registrationForm);
    DTO.User GetUser(int id);

}

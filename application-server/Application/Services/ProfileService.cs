public class ProfileService : IProfileService {

    private readonly IProfileQueries queries;
    private readonly IAuthenticationService authentication;
    private readonly IAuthenticationQueries authenticationQueries;

    public ProfileService(IProfileQueries queries,
            IAuthenticationService authentication, IAuthenticationQueries authenticationQueries) {
        this.queries = queries;
        this.authentication = authentication;
        this.authenticationQueries = authenticationQueries;
    }

    public DTO.User GetUser(int id) {
        // Search user in the DB
        Entity.User user = queries.FindFromUserId(id);

        // Return user or null
        if (user == null) return null;
        return (new User(user)).ToDto();
    }

    public bool UpdateProfile(int userId, DTO.ProfileUpdate updateForm) {
        bool errors = false;

        // Check update form validity
        if (!IsUpdateFormValid(updateForm)) return false;

        // Change password
        if (!string.IsNullOrWhiteSpace(updateForm.Password)) {
            // Retrieve salt and hashed password
            var salt = authentication.GenerateSalt();
            var hash = authentication.HashPassword(salt, updateForm.Password);

            // Update salt and password
            if (!queries.UpdateSaltAndPassword(userId, salt, hash))
                errors = true;;
        }

        // Update username
        if (!string.IsNullOrWhiteSpace(updateForm.Username)) {
            if (!queries.UpdateUsername(userId, updateForm.Username))
                errors = true;
        }

        // Update email
        if (!string.IsNullOrWhiteSpace(updateForm.Email)) {
            if (!queries.UpdateEmail(userId, updateForm.Email))
                errors = true;
        }

        return !errors;
    }

    public bool IsUpdateFormValid(DTO.ProfileUpdate updateForm) {
        string username = updateForm.Username;
        string password = updateForm.Password;
        string email = updateForm.Email;

        // No white space allowed
        if (username.Contains(' ')) return false;
        if (email.Contains(' ')) return false;

        // Check parameters lenghts
        if (username.Length < 4 || username.Length > 32) return false;
        if (password.Length < 8 || password.Length > 32) return false;
        if (email.Length < 5 || email.Length > 32) return false;

        // Check that email has right format
        if (!authentication.IsValidEmail(email)) return false;
        if (authentication.IsValidEmail(username)) return false;

        // Check that username and email are unique
        if (authenticationQueries.FindFromEmail(email.ToLowerInvariant()) != null) return false;
        if (authenticationQueries.FindFromUsername(username) != null) return false;

        // All checks have been passed
        return true;
    }

}

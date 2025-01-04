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

    public User GetUser(int id) {
        // Search user in the DB
        Entity.User user = queries.FindFromUserId(id);

        // Return user or null
        if (user == null) return null;
        return new User(user);
    }

    public bool UpdateProfile(int userId, DTO.ProfileUpdate updateForm) {
        bool errors = false;

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
        var username = updateForm.Username;
        var email = updateForm.Email;

        // Check username uniqueness
        if (!string.IsNullOrWhiteSpace(username)) {
            if (authenticationQueries.FindFromUsername(username) != null)
                return false;
        }

        // Check email uniqueness
        if (!string.IsNullOrWhiteSpace(email)) {
            if (authenticationQueries.FindFromEmail(email.ToLowerInvariant()) != null)
                return false;
        }

        // Checks passed
        return true;
    }

}

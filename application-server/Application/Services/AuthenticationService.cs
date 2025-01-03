using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

public class AuthenticationService : IAuthenticationService {

    private readonly IAuthenticationQueries queries;
    private string jwtIssuer;
    private string jwtAudience;
    private string jwtSecret;
    private int expirationHours;

    public AuthenticationService(IAuthenticationQueries queries, IConfiguration configuration) {
        this.queries = queries;
        this.jwtIssuer = configuration["Jwt:Issuer"];
        this.jwtAudience = configuration["Jwt:Audience"];
        this.jwtSecret = configuration["Jwt:Secret"];
        this.expirationHours = Convert.ToInt32(configuration["Jwt:ExpirationHours"]);
    }

    public bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm) {
        string username = registrationForm.Username;
        string password = registrationForm.Password;
        string userType = registrationForm.UserType;
        string email = registrationForm.Email;

        // Check that fields are not null
        if (string.IsNullOrWhiteSpace(username)) return false;
        if (string.IsNullOrWhiteSpace(password)) return false;
        if (string.IsNullOrWhiteSpace(userType)) return false;
        if (string.IsNullOrWhiteSpace(email)) return false;

        // No white space allowed
        if (username.Contains(' ')) return false;
        if (email.Contains(' ')) return false;

        // Check parameters lenghts
        if (username.Length < 4 || username.Length > 32) return false;
        if (password.Length < 8 || password.Length > 32) return false;
        if (email.Length < 5 || email.Length > 32) return false;

        // Check that email has right format
        if (!IsValidEmail(email)) return false;
        if (IsValidEmail(username)) return false;

        // Check that user type is convertible
        try {
            User.UserTypeFromString(userType);
        } catch {
            return false;
        }

        // Check that username and email are unique
        if (queries.FindFromEmail(email.ToLowerInvariant()) != null) return false;
        if (queries.FindFromUsername(username) != null) return false;

        // All checks have been passed
        return true;
    }

    public bool RegisterUser(DTO.RegistrationForm registrationForm) {
        // Retrieve salt and hashed password
        var salt = GenerateSalt();
        var hash = HashPassword(salt, registrationForm.Password);

        // Convert DTO to entity
        var user = new Entity.User {
            Username = registrationForm.Username,
            Email = registrationForm.Email.ToLowerInvariant(),
            Salt = salt,
            HashedPassword = hash,
            UserType = registrationForm.UserType
        };
        return queries.RegisterUser(user);
    }

    public DTO.User ValidateCredentials(DTO.Credentials credentials) {
        // Check that fields are not null
        if (string.IsNullOrWhiteSpace(credentials.Username)) return null;
        if (string.IsNullOrWhiteSpace(credentials.Password)) return null;

        // Search for credentials in the DB
        Entity.User user;
        if (!IsValidEmail(credentials.Username))
            user = queries.FindFromUsername(credentials.Username);
        else
            user = queries.FindFromEmail(credentials.Username);

        // Return null if user not found
        if (user == null) return null;

        // Hash password with user salt
        var hash = HashPassword(user.Salt, credentials.Password);

        // Return the user, or null if password doesn't match
        if (hash.Equals(user.HashedPassword))
            return (new User(user)).ToDto();
        else
            return null;
   }

    public string GenerateToken(DTO.User user) {
        // Define claim fields in the token
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        // Defince token encryption parameters
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Create and return token
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(expirationHours),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateSalt() {
        // Generate a cryptographic random salt
        byte[] saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string salt, string password) {
        // Combine the salt with the password and hash them
        using var sha256 = SHA256.Create();
        string saltedPassword = salt + password;
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashBytes);
    }

    public bool IsValidEmail(string email) {
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase);
    }

}

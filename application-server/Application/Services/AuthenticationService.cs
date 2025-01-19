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

    public bool IsCompanyRegistrationValid(DTO.RegistrationFormCompany registrationForm) {
        // Check that username and email are unique
        if (queries.FindCompanyFromEmail(registrationForm.Email.ToLowerInvariant()) != null) return false;
        if (queries.FindCompanyFromUsername(registrationForm.Username) != null) return false;

        // Checks passed
        return true;
    }

    public bool RegisterCompany(DTO.RegistrationFormCompany registrationForm) {
        // Retrieve salt and hashed password
        var salt = GenerateSalt();
        var hash = HashPassword(salt, registrationForm.Password);

        // Convert DTO to entity
        Entity.Company user = new Company(registrationForm).ToEntity();
        return queries.RegisterCompany(user);
    }

    public bool IsStudentRegistrationValid(DTO.RegistrationFormStudent registrationForm) {
        // Check that username and email are unique
        if (queries.FindStudentFromEmail(registrationForm.Email.ToLowerInvariant()) != null) return false;
        if (queries.FindStudentFromUsername(registrationForm.Username) != null) return false;

        // Checks passed
        return true;
    }

    public bool RegisterStudent(DTO.RegistrationFormStudent registrationForm) {
        // Retrieve salt and hashed password
        var salt = GenerateSalt();
        var hash = HashPassword(salt, registrationForm.Password);

        // Convert DTO to entity
        Entity.Student user = new Student(registrationForm).ToEntity();
        return queries.RegisterStudent(user);
    }

    public User ValidateCredentials(DTO.Credentials credentials) {
        // Search for credentials in the DB
        User user;
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
            return user;
        else
            return null;
   }

    public string GenerateToken(User user) {
        // Define claim fields in the token
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserType.ToString()),
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

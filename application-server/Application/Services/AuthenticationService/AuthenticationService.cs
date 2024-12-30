using System;
using System.Text;
using System.Security.Claims;
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
        // No dotenv here plz
        this.jwtSecret = Environment.GetEnvironmentVariable("JWT_TOKEN") ?? configuration["Jwt:Key"];
        this.expirationHours = Convert.ToInt32(configuration["Jwt:ExpirationHours"]);
    }

    public bool IsRegistrationFormValid(DTO.RegistrationForm registrationForm) {
        // TODO check that username and email are unique
        return true;
    }

    public bool RegisterUser(DTO.RegistrationForm registrationForm) {
        // Convert DTO to entity
        var user = new Entity.User {
            Username = registrationForm.Username,
            Email = registrationForm.Email,
            HashedPassword = registrationForm.Password, // TODO hash and salt the password
            UserType = registrationForm.UserType
        };
        return queries.RegisterUser(user);
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

    public DTO.User GetUser(int id) {
        // TODO check if query goes well
        var userEntity = queries.GetUser(id);
        var userDto = new DTO.User {
            Id = userEntity.Id,
            Username = userEntity.Username,
            UserType = userEntity.UserType,
            Email = userEntity.Email
        };
        return userDto;
    }

}

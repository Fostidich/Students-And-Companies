using Microsoft.AspNetCore.Mvc;

//TODO add check token validity endpoint
//TODO add logout endpoint
[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase {

    private readonly IAuthenticationService authentication;

    public AuthenticationController(IAuthenticationService service) {
        this.authentication = service;
    }

    [HttpPost("register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult Register([FromBody] DTO.RegistrationForm registrationForm) {
        // Check registration form validity
        if (!authentication.IsRegistrationFormValid(registrationForm))
            return BadRequest("Validation error\n");

        // Add user data to DB
        if (authentication.RegisterUser(registrationForm))
            return Ok("User registered\n");
        else
            return StatusCode(500, "Internal server error\n");
    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult Login([FromBody] DTO.Credentials credentials) {
        // Check that credentials are correct
        User user = authentication.ValidateCredentials(credentials);
        if (user == null)
            return Unauthorized("Invalid credentials\n");

        // Generate token from user data
        string token = authentication.GenerateToken(user);
        return Ok(new { token });
    }

}

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase {

    private readonly IAuthenticationService authentication;

    public AuthenticationController(IAuthenticationService service) {
        this.authentication = service;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] DTO.RegistrationForm registrationForm) {

        // Check registration form validity
        if (!authentication.IsRegistrationFormValid(registrationForm))
            return BadRequest("Validation error.\n");

        // Add user data to DB
        if (authentication.RegisterUser(registrationForm))
            return Ok();
        else
            return StatusCode(500, "Internal server error.\n");
    }

    [HttpPost("verification/{id}")]
    public IActionResult Verification(int id) {
        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] DTO.Credentials credentials) {
        return Ok(credentials);
    }

    [HttpGet("users/{id}")]
    public IActionResult GetUser(int id) {
        // TODO check if user exists
        return Ok(authentication.GetUser(id));
    }

}

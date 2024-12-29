using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase {

    [HttpPost("register")]
    public IActionResult Register([FromBody] DTO.RegistrationForm registrationForm) {

        // Check registration form validity
        if (!AuthenticationService.IsRegistrationFormValid(registrationForm))
            return BadRequest("Validation error.\n");

        // Add user data to DB
        if (AuthenticationService.RegisterUser(registrationForm))
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
        return Ok(AuthenticationService.GetUser(id));
    }

}

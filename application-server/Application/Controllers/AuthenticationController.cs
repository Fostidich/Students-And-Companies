using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase {

    private readonly IAuthenticationService authentication;

    public AuthenticationController(IAuthenticationService service) {
        this.authentication = service;
    }

    [HttpPost("register/company")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult RegisterCompany([FromBody] DTO.RegistrationFormCompany registrationForm) {
        // Check registration form validity
        if (!authentication.IsCompanyRegistrationValid(registrationForm))
            return BadRequest("Validation error\n");

        // Add user data to DB
        if (authentication.RegisterCompany(registrationForm))
            return Ok("User registered\n");
        else
            return StatusCode(500, "Internal server error\n");
    }

    [HttpPost("register/student")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult RegisterStudent([FromBody] DTO.RegistrationFormStudent registrationForm) {
        // Check registration form validity
        if (!authentication.IsStudentRegistrationValid(registrationForm))
            return BadRequest("Validation error\n");

        // Add user data to DB
        if (authentication.RegisterStudent(registrationForm))
            return Ok("User registered\n");
        else
            return StatusCode(500, "Internal server error\n");
    }


    [HttpPost("validation/{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult ValidationCode(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
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
        return Ok(new DTO.SuccessfulLogin {
            Token = token,
            UserType = user.UserType.ToString()
        });
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult Logout() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public IActionResult CheckToken() {
        return Ok();
    }

}

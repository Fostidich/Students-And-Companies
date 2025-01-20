using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase {

    private readonly IAuthenticationService authentication;

    public AuthenticationController(IAuthenticationService service) {
        this.authentication = service;
    }

    [HttpPost("register/company")]
    [SwaggerOperation(Summary = "Register a company", Description = "A new user is registered into the system from the information in the registration form found in the request body.")]
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
    [SwaggerOperation(Summary = "Register a student", Description = "A new user is registered into the system from the information in the registration form found in the request body.")]
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
    [SwaggerOperation(Summary = "Log in a user from credentials", Description = "Upon validating credentials, the login token is returned along with the user type. If the username field of the credentials contains the email, the validation will still be processed correctly.")]
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
    [SwaggerOperation(Summary = "Check log in token validity", Description = "The authorization token, generated at log in time, if found in the header is validated, generating either an Ok or Unauthorized response.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public IActionResult CheckToken() {
        return Ok();
    }

}

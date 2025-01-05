using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/internship")]
public class InternshipController : ControllerBase {

    private readonly IInternshipService internship;

    public InternshipController(IInternshipService service) {
        this.internship = service;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetInternships() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("feedback")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult CreateFeedback() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

}

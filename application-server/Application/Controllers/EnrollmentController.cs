using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/enrollment")]
public class EnrollmentController : ControllerBase {

    private readonly IEnrollmentService enrollment;

    public EnrollmentController(IEnrollmentService service) {
        this.enrollment = service;
    }

    [HttpGet("applications")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetApplications() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet("applications/{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetApplication(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("applications")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult CreateApplication() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("applications/{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult AcceptApplication(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("questionnaires")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult BuildQuestionnaire() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet("questionnaires/{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetQuestionnaire(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("questionnaires/{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult FillQuestionnaire(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet("proposals")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetProposals() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("proposals")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult CreateProposals() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("proposals/{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult AcceptProposal(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

}

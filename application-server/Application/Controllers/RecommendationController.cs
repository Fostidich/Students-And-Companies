using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/recommendation")]
public class RecommendationController : ControllerBase {

    private readonly IRecommendationService recommendation;

    public RecommendationController(IRecommendationService service) {
        this.recommendation = service;
    }

    [HttpGet("advertisements")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetAdvertisements() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("advertisements")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult CreateAdvertisement() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet("advertisements/{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetAdvertisement(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet("candidates")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetCandidates() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet("candidates/{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetCandidate(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

}

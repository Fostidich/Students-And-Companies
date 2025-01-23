using System;
using System.Security.Claims;
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
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpPost("{id}/feedback")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult CreateFeedback(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }
    
    [HttpGet("Advertisement/{advertisementId}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetInternshipFromAdvertisement(int advertisementId) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        return StatusCode(501, "Feature not yet implemented\n");
    }
}

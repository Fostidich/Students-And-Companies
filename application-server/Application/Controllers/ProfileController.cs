using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase {

    private readonly IProfileService profile;

    public ProfileController(IProfileService service) {
        this.profile = service;
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetUserFromToken() {
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId;
        try {
            userId = Convert.ToInt32(userIdStr);
        } catch {
            return BadRequest("Validation error\n");
        }

        // Find and return user data
        DTO.User user = profile.GetUser(userId);
        if (user == null)
            return StatusCode(500, "Internal server error\n");
        else
            return Ok(user);
    }

    [Authorize]
    [HttpPost]
    public IActionResult UpdateProfile([FromBody] DTO.ProfileUpdate updateForm) {
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId;
        try {
            userId = Convert.ToInt32(userIdStr);
        } catch {
            return BadRequest("Validation error\n");
        }

        // Check update form validity
        if (!profile.IsUpdateFormValid(updateForm))
            return BadRequest("Validation error\n");

        // Update profile
        if (profile.UpdateProfile(userId, updateForm))
            return Ok("Profile updated\n");
        else
            return StatusCode(500, "Internal server error\n");
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetUserFromId(int id) {
        // Find and return user data
        DTO.User user = profile.GetUser(id);
        if (user == null)
            return StatusCode(500, "Internal server error\n");
        else
            return Ok(user);
    }

}

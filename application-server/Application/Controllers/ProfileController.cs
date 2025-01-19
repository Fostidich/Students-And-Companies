using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase {

    private readonly IProfileService profile;

    public ProfileController(IProfileService service) {
        this.profile = service;
    }

    [HttpGet("company")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult GetCompanyFromToken() {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Find and return user data
        DTO.Company user = profile.GetCompany(userId)?.ToDto();
        if (user == null)
            return StatusCode(500, "Internal server error\n");
        else
            return Ok(user);
    }

    [HttpGet("student")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult GetStudentFromToken() {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Find and return user data
        DTO.Student user = profile.GetStudent(userId)?.ToDto();
        if (user == null)
            return StatusCode(500, "Internal server error\n");
        else
            return Ok(user);
    }

    [HttpPost("company")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult UpdateProfileCompany([FromBody] DTO.ProfileUpdateCompany updateForm) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        // Check update form validity
        if (!profile.IsCompanyUpdateFormValid(updateForm))
            return BadRequest("Validation error\n");

        // Update profile
        if (profile.UpdateProfileCompany(userId, updateForm))
            return Ok("Profile updated\n");
        else
            return StatusCode(500, "Internal server error\n");
    }

    [HttpPost("student")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult UpdateProfileStudent([FromBody] DTO.ProfileUpdateStudent updateForm) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        // Check update form validity
        if (!profile.IsStudentUpdateFormValid(updateForm))
            return BadRequest("Validation error\n");

        // Update profile
        if (profile.UpdateProfileStudent(userId, updateForm))
            return Ok("Profile updated\n");
        else
            return StatusCode(500, "Internal server error\n");
    }

    [HttpGet("company/{id}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public IActionResult GetCompanyFromId(int id) {
        // Find and return user data
        DTO.Company user = profile.GetCompany(id)?.ToDto();
        if (user == null)
            return StatusCode(500, "Internal server error\n");
        else
            return Ok(user);
    }

    [HttpGet("student/{id}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public IActionResult GetStudentFromId(int id) {
        // Find and return user data
        DTO.Student user = profile.GetStudent(id)?.ToDto();
        if (user == null)
            return StatusCode(500, "Internal server error\n");
        else
            return Ok(user);
    }

    [HttpGet("cv")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult DownloadCvFromToken() {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Retrieve CV from user ID
        IFormFile cv = profile.RetrieveCvFile(userId);

        // File is null if not present
        if (cv == null)
            return NotFound("User has not uploaded the CV\n");
        else
            return File(cv.OpenReadStream(), cv.ContentType, cv.FileName);
    }

    [HttpGet("cv/{id}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult DownloadCv(int id) {
        // Retrieve CV from user ID
        IFormFile cv = profile.RetrieveCvFile(id);

        // File is null if not present
        if (cv == null)
            return NotFound("User has not uploaded the CV");
        else
            return File(cv.OpenReadStream(), cv.ContentType, cv.FileName);
    }

    [HttpPost("cv")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult UploadCv(IFormFile file) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Check PDF validity
        if (!profile.CheckCvValidity(file))
            return BadRequest("Invalid file\n");

        // Store file
        if (profile.StoreCvFile(userId, file))
            return Ok("CV successfully stored\n");
        else
            return StatusCode(500, "Internal server error\n");
    }

    [HttpPost("cv/delete")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCv() {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Delete CV
        if (profile.DeleteCv(userId))
            return Ok("CV deleted\n");
        else
            return NotFound("No CV found\n");
    }

    [HttpPost("delete")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult DeleteUser() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

}

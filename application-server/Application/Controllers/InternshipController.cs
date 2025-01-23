using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/internship")]
public class InternshipController : ControllerBase {

    private readonly IInternshipService internship;

    public InternshipController(IInternshipService service) {
        this.internship = service;
    }
    

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Get internships for a student", Description = "Get internships for a student. The internships are filtered based on the student ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult GetInternships() {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        DTO.Internship internships;
        Internship checkInternships;
        
        checkInternships = internship.GetInternshipForStudent(userId);
        
        if (checkInternships == null)
            return StatusCode(500, "Internal server error\n");
        
        internships = checkInternships.ToDto();
        
        return Ok(internships);
    }

    
    [HttpPost("{internshipId}/feedback/Student")]
    [Authorize]
    [SwaggerOperation(Summary = "Create feedback for a student", Description = "Create feedback for a student. The feedback is created based on the internship ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult CreateStudentFeedback(int internshipId, [FromBody] DTO.Feedback feedback) {
        
        if (internshipId <= 0) return BadRequest("Invalid id\n");
        
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        bool created = internship.CreateStudentFeedback(internshipId, feedback, userId);
        
        if (created)
            return Ok("Feedback created\n");
        
        return BadRequest("You can't create this feedback\n");
        
    }
    
    
    [HttpPost("{internshipId}/feedback/Company")]
    [Authorize]
    [SwaggerOperation(Summary = "Create feedback for a company", Description = "Create feedback for a company. The feedback is created based on the internship ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult CreateCompanyFeedback(int internshipId, [FromBody] DTO.Feedback feedback) {
        
        if (internshipId <= 0) return BadRequest("Invalid id\n");
        
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        bool created = internship.CreateCompanyFeedback(internshipId, feedback, userId);
        
        if (created)
            return Ok("Feedback created\n");
        
        return BadRequest("You can't create this feedback\n");
        
    }
    
    
    [HttpGet("{internshipId}/feedback/Student")]
    [Authorize]
    [SwaggerOperation(Summary = "Get the student feedback", Description = "Get the student feedback. The feedback is filtered based on the internship ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult GetStudentFeedback(int internshipId) {
        
        if (internshipId <= 0) return BadRequest("Invalid id\n");
        
        // Get role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        DTO.Feedback feedback= internship.GetStudentFeedback(internshipId, userId, role);
        
        if(feedback != null)
            return Ok(feedback);
        
        return BadRequest("You can't get this feedback\n");
        
    }
    
    
    [HttpGet("{internshipId}/feedback/Company")]
    [Authorize]
    [SwaggerOperation(Summary = "Get the company feedback", Description = "Get the company feedback. The feedback is filtered based on the internship ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult GetCompanyFeedback(int internshipId) {
        
        if (internshipId <= 0) return BadRequest("Invalid id\n");
        
        // Get role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        DTO.Feedback feedback= internship.GetCompanyFeedback(internshipId, userId, role);
        
        if(feedback != null)
            return Ok(feedback);
        
        return BadRequest("You can't get this feedback\n");
        
    }
    
    
    [HttpGet("Advertisement/{advertisementId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Get internships from an advertisement", Description = "Get internships from an advertisement. The internships are filtered based on the advertisement ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult GetInternshipFromAdvertisement(int advertisementId) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");
        
        
        List<DTO.Internship> internships;
        List<Internship> checkInternships;
        
        checkInternships = internship.GetInternshipFromAdvertisement(advertisementId);
        
        if (checkInternships == null)
            return StatusCode(500, "Internal server error\n");
        
        internships = checkInternships.Select(internship => internship.ToDto()).ToList();
        
        return Ok(internships);
    }
}

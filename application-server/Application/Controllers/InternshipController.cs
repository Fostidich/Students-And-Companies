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
    [ProducesResponseType(404)]
    public IActionResult GetInternships() {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        List<DTO.Internship> internships;
        List<Internship> checkInternships;

        checkInternships = internship.GetInternshipForStudent(userId);

        if (checkInternships == null)
            return StatusCode(500, "Internal server error\n");
        
        if (checkInternships.Count == 0)
            return NotFound("No internships found\n");

        internships = checkInternships.Select(i => i.ToDto()).ToList();

        return Ok(internships);
    }


    [HttpPost("{internshipId}/feedback/student")]
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

        return BadRequest("You can't create this feedback, and there are 3 possible reasons: " +
                          "1. The internship isn't finished yet; " +
                          "2. You don't have the permission to create this feedback; " +
                          "3. The feedback already exists\n");

    }


    [HttpPost("{internshipId}/feedback/company")]
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

        return BadRequest("You can't create this feedback, and there are 3 possible reasons: " +
                          "1. The internship isn't finished yet; " +
                          "2. You don't have the permission to create this feedback; " +
                          "3. The feedback already exists\n");

    }


    [HttpGet("{internshipId}/feedback/student")]
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

        return BadRequest("You don't have the permission to see this feedback or the feedback doesn't exist yet\n");

    }


    [HttpGet("{internshipId}/feedback/company")]
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

        return BadRequest("You don't have the permission to see this feedback or the feedback doesn't exist yet\n");

    }


    [HttpGet("advertisements/{advertisementId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Get internships from an advertisement", Description = "Get internships from an advertisement. The internships are filtered based on the advertisement ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult GetInternshipFromAdvertisement(int advertisementId) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);


        List<DTO.Internship> internships;
        List<Internship> checkInternships;

        checkInternships = internship.GetInternshipFromAdvertisement(advertisementId, userId);

        if (checkInternships == null)
            return BadRequest("You don't have the permission to see this internship\n");

        internships = checkInternships.Select(internship => internship.ToDto()).ToList();

        return Ok(internships);
    }
    
    
    [HttpPost("delete/{internshipId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Only for test, don't use", Description = "Delete an internship. The internship is deleted based on the internship ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult DeleteInternship(int internshipId) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        bool deleted = internship.DeleteInternship(internshipId, userId, role);

        if (deleted)
            return Ok("Internship deleted\n");

        return NotFound("You don't have the permission to delete this internship\n");
    }
    
    
    [HttpPost("delete/feedback/{internshipId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Delete your feedback for the internship", Description = "Delete your feedback for the internship. The feedback is deleted based on the internship ID.")]   
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult DeleteFeedback(int internshipId) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        bool deleted = internship.DeleteFeedback(internshipId, userId, role);

        if (deleted)
            return Ok("Feedback deleted\n");

        return NotFound("You don't have this feedback\n");
    }
}

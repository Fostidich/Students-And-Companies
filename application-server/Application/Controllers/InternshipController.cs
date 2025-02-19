using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/internship")]
public class InternshipController : ControllerBase
{

    private readonly IInternshipService internship;

    public InternshipController(IInternshipService service)
    {
        this.internship = service;
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Get internships data of the student", Description = "Return internships information of the student with the provided ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetInternships()
    {
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
            return NotFound("No internship found\n");

        internships = checkInternships.Select(i => i.ToDto()).ToList();

        return Ok(internships);
    }

    [HttpPost("{internshipId}/feedback/student")]
    [Authorize]
    [SwaggerOperation(Summary = "Create a feedback for an internship", Description = "The student creates a feedback for the internship with the provied ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult CreateStudentFeedback(int internshipId, [FromBody] DTO.Feedback feedback)
    {
        // Check id validity
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

        return BadRequest("The feedback cannot be created\n");
    }

    [HttpPost("{internshipId}/feedback/company")]
    [Authorize]
    [SwaggerOperation(Summary = "Create a feedback for an internship", Description = "The company creates a feedback for the internship with the provied ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult CreateCompanyFeedback(int internshipId, [FromBody] DTO.Feedback feedback)
    {
        // Check id validity
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

        return BadRequest("The feedback cannot be created\n");
    }

    [HttpGet("{internshipId}/feedback/student")]
    [Authorize]
    [SwaggerOperation(Summary = "Get the feedback made by the student", Description = "Return the feedback made by the student for the internship with the provided ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult GetStudentFeedback(int internshipId)
    {
        // Check id validity
        if (internshipId <= 0)
            return BadRequest("Invalid id\n");

        // Get role
        string role = User.FindFirst(ClaimTypes.Role).Value;

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        DTO.Feedback feedback = internship.GetStudentFeedback(internshipId, userId, role);

        if (feedback != null)
            return Ok(feedback);

        return BadRequest("The feedback cannot be retrieved\n");
    }

    [HttpGet("{internshipId}/feedback/company")]
    [Authorize]
    [SwaggerOperation(Summary = "Get the feedback made by the company", Description = "Return the feedback made by the company for the internship with the provided ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult GetCompanyFeedback(int internshipId)
    {
        // Check id validity
        if (internshipId <= 0) return BadRequest("Invalid id\n");

        // Get role
        string role = User.FindFirst(ClaimTypes.Role).Value;

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        DTO.Feedback feedback = internship.GetCompanyFeedback(internshipId, userId, role);

        if (feedback != null)
            return Ok(feedback);

        return BadRequest("The feedback cannot be retrieved\n");
    }

    [HttpGet("advertisements/{advertisementId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Get ongoing internships from the advertisement", Description = "Return the ongoing internships relative to the advertisement with the provided ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult GetInternshipFromAdvertisement(int advertisementId)
    {
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
            return BadRequest("The internship data cannot be retrieved\n");

        internships = checkInternships.Select(internship => internship.ToDto()).ToList();

        return Ok(internships);
    }

    [HttpPost("delete/{internshipId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Delete an internship", Description = "The internship with the provided ID is deleted.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult DeleteInternship(int internshipId)
    {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        bool deleted = internship.DeleteInternship(internshipId, userId, role);

        if (deleted)
            return Ok("Internship deleted\n");

        return NotFound("No internship found\n");
    }

    [HttpPost("delete/feedback/{internshipId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Delete the feedback made for an internship", Description = "The feedback previously submitted on the internship with the provided ID is deleted.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult DeleteFeedback(int internshipId)
    {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        bool deleted = internship.DeleteFeedback(internshipId, userId, role);

        if (deleted)
            return Ok("Feedback deleted\n");

        return NotFound("No feedback found\n");
    }

}

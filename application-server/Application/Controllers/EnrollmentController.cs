using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/enrollment")]
public class EnrollmentController : ControllerBase {

    private readonly IEnrollmentService enrollment;

    public EnrollmentController(IEnrollmentService service) {
        this.enrollment = service;
    }

    [HttpPost("applications/{advertisementId}")]
    [SwaggerOperation(Summary = "Make an application request", Description = "An application request is register to the advertisement with the provided ID.")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateApplication([FromBody] DTO.ApplicationRegistration application, int advertisementId) {
        // Check id validity
        if (advertisementId <= 0)
            return BadRequest("Invalid advertisement ID\n");

        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Check that advertisement exists
        if (enrollment.GetAdvertisement(advertisementId) == null)
            return BadRequest("Advertisement not found\n");

        // Check that student is not already in an internship
        if (enrollment.GetInternship(userId) != null)
            return BadRequest("Student already in an internship\n");

        // Check application uniqueness
        if (enrollment.GetApplication(userId, advertisementId) != null)
            return BadRequest("Application already submitted\n");

        // Create application
        if (enrollment.CreateApplication(userId, advertisementId, application.QuestionnaireAnswer))
            return Ok("Application registered\n");
        else
            return StatusCode(500, "Internal server error\n");
    }

    [HttpGet("applications/{advertisementId}")]
    [SwaggerOperation(Summary = "Get pending applications", Description = "The requested applications, in the pending status, relative to the advertisement with the provided ID are returned.")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetPendingApplications(int advertisementId) {
        // Check id validity
        if (advertisementId <= 0)
            return BadRequest("Invalid advertisement ID\n");

        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Check that advertisement exists
        var advertisement = enrollment.GetAdvertisement(advertisementId);
        if (advertisement == null)
            return NotFound("Advertisement not found\n");

        // Check that company is the proprietary
        if (advertisement.CompanyId != userId)
            return BadRequest("Not proprietary of advertisement\n");

        // Retrieve applications in pending status
        List<Application> applications = enrollment.GetPendingApplications(advertisementId);

        // Check if error occurred
        if (applications == null)
            return StatusCode(500, "Internal server error\n");

        // Check there actually are applications
        if (applications.Count == 0)
            return NotFound("No applications found\n");

        // Return the applications as DTOs
        return Ok(applications.Select(a => a.ToDto()).ToList());
    }

    [HttpPost("accept/{applicationId}")]
    [SwaggerOperation(Summary = "Accept an application", Description = "The applications with the provided ID is accepted, the internship is instantiated with the provided starting date, and the student is thus notified.")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult AcceptApplication([FromBody] DTO.Date date, int applicationId) {
        // Check id validity
        if (applicationId <= 0)
            return BadRequest("Invalid application ID\n");

        // Check date validity
        if (!enrollment.CheckStartDateValidity(date.DateTime))
            return BadRequest("Starting date outside valid range\n");

        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Retrieve application
        Application application = enrollment.GetApplication(applicationId);

        // Check for errors
        if (application == null)
            return NotFound("Application not found\n");

        // Check if application is pending
        if (application.Status != "PENDING")
            return BadRequest("Application already accepted or rejected\n");

        // Check that student is not already in an internship
        if (enrollment.GetInternship(application.StudentId) != null)
            return BadRequest("Student already in an internship\n");

        // Change application status
        if (!enrollment.AcceptApplication(applicationId))
            return StatusCode(500, "Internal server error\n");

        // Create internship
        if (!enrollment.CreateInternship(application.StudentId, userId,
                    application.AdvertisementId, date.DateTime))
            return StatusCode(500, "Internal server error\n");

        // Update advertisement free spots
        if (!enrollment.UpdateAdvertisementSpots(application.AdvertisementId))
            return StatusCode(500, "Internal server error\n");

        // Reject all other student application
        if (!enrollment.RejectAllApplications(application.StudentId))
            return StatusCode(500, "Internal server error\n");

        // Notify student
        if (!enrollment.NotifyStudent(application.StudentId, application.AdvertisementId, true))
            return StatusCode(500, "Internal server error\n");

        return Ok("Internship instantiated\n");
    }

    [HttpPost("reject/{applicationId}")]
    [SwaggerOperation(Summary = "Reject an application", Description = "The applications with the provided ID is rejected, and the student is thus notified.")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult RejectApplication(int applicationId) {
        // Check id validity
        if (applicationId <= 0)
            return BadRequest("Invalid application ID\n");

        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId = Convert.ToInt32(userIdStr);

        // Retrieve application
        Application application = enrollment.GetApplication(applicationId);

        // Check for errors
        if (application == null)
            return NotFound("Application not found\n");

        // Check if application is pending
        if (application.Status != "PENDING")
            return BadRequest("Application already accepted or rejected\n");

        // Change application status
        if (!enrollment.RejectApplication(applicationId))
            return StatusCode(500, "Internal server error\n");

        // Notify student
        if (!enrollment.NotifyStudent(application.StudentId, application.AdvertisementId, false))
            return StatusCode(500, "Internal server error\n");

        return Ok("Application rejected\n");
    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/recommendation")]
public class RecommendationController : ControllerBase
{

    private readonly IRecommendationService recommendation;

    public RecommendationController(IRecommendationService service)
    {
        this.recommendation = service;
    }

    [HttpGet("advertisements")]
    [Authorize]
    [SwaggerOperation(Summary = "Get a list of advertisements", Description = "The list of advertisements differs based on the user role: if the user is a student, the advertisements are recommendation based; if the user is a company, the advertisements are the ones the company has posted.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetAdvertisements()
    {
        // Get user role from authentication token
        string role = User.FindFirst(ClaimTypes.Role)?.Value;

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        List<DTO.Advertisement> adv;
        List<Advertisement> checkAdv;

        if (role == UserType.Student.ToString())
        {
            checkAdv = recommendation.GetAdvertisementsForStudent(userId);

            if (checkAdv == null)
                return StatusCode(500, "Internal server error\n");

            if (checkAdv.Count == 0)
                return NotFound("No advertisements found\n");

            adv = checkAdv.Select(ad => ad.ToDto()).ToList();

        }
        else if (role == UserType.Company.ToString())
        {
            checkAdv = recommendation.GetAdvertisementsOfCompany(userId);

            if (checkAdv == null)
                return StatusCode(500, "Internal server error\n");

            if (checkAdv.Count == 0)
                return NotFound("No advertisements found\n");

            adv = checkAdv.Select(ad => ad.ToDto()).ToList();
        }
        else
            return BadRequest("Invalid user role\n");

        return Ok(adv);
    }

    [HttpPost("advertisements")]
    [Authorize]
    [SwaggerOperation(Summary = "Create a new advertisement", Description = "A company creates a new advertisement based on the provided data.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult CreateAdvertisement([FromBody] DTO.AdvertisementRegistration advertisement)
    {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        // Add advertisement data to DB
        if (recommendation.CreateAdvertisement(userId, advertisement))
        {
            return Ok("Advertisement registered\n");
        }

        return BadRequest("Invalid information provided\n");
    }

    [HttpGet("advertisements/{advertisementId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Get the advertisement data", Description = "Return the information relative to the advertisement with the provided ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetAdvertisement(int advertisementId)
    {
        // Check ID validity
        if (advertisementId <= 0)
            return BadRequest("Invalid id\n");

        DTO.Advertisement adv = recommendation.GetAdvertisement(advertisementId)?.ToDto();

        if (adv == null)
            return NotFound("Advertisement not found\n");

        return Ok(adv);
    }

    [HttpGet("candidates/advertisements/{advertisementId}")]
    [SwaggerOperation(Summary = "Get students recommended for an advertisement", Description = "Return the candidate students that have been suggested by the recommendation system based on their skills and the advertisement requirements.")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult GetRecommendedCandidates(int advertisementId)
    {
        // Check ID validity
        if (advertisementId <= 0)
            return BadRequest("Invalid id\n");

        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        List<Student> checkstudents = recommendation.GetRecommendedCandidates(userId, advertisementId);

        if (checkstudents == null)
            return BadRequest("Advertisement cannot be retrieved\n");

        if (checkstudents.Count == 0)
            return NotFound("No student found\n");

        List<DTO.Student> students = checkstudents.Select(student => student.ToDto()).ToList();

        return Ok(students);
    }

    [HttpPost("suggestions/advertisement/{advertisementId}/student/{studentId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Invite a student to apply for an advertisement", Description = "The company suggests a student to apply for a specific advertisement. The student is therefore notified.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult CreateSuggestion(int advertisementId, int studentId)
    {
        // Check ID validity
        if (advertisementId <= 0 || studentId <= 0)
            return BadRequest("Invalid id\n");

        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        bool suggestionCreated = recommendation.CreateSuggestionsForStudent(advertisementId, studentId, userId);

        if (suggestionCreated)
            return Ok("Invitation sent\n");

        return NotFound("Student or advertisement not found\n");
    }

    [HttpPost("delete/{advertisementId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Delete an advertisement", Description = "The advertisement with the provided ID is deleted.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteAdvertisement(int advertisementId)
    {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");

        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        bool deleted = recommendation.DeleteAdvertisement(advertisementId, userId);

        if (deleted)
            return Ok("Advertisement deleted\n");

        return NotFound("No advertisement found\n");
    }

}

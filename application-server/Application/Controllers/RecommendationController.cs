using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/recommendation")]
public class RecommendationController : ControllerBase {

    private readonly IRecommendationService recommendation;

    public RecommendationController(IRecommendationService service) {
        this.recommendation = service;
    }

    [HttpGet("advertisements")]
    [Authorize]
    [SwaggerOperation(Summary = "Get distinct advertisements for a student or for a company", Description = "Get advertisements for a student or for a company. The advertisements are distinct and are filtered based on the user role: if the user is a student, the advertisements are a recommendation based on the student's skills; if the user is a company, the advertisements are all company's advertisements.")]
    [ProducesResponseType(400)]
    public IActionResult GetAdvertisements(){
        // Get user role from authentication token
        string role = User.FindFirst(ClaimTypes.Role)?.Value;
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);

        if (string.IsNullOrEmpty(role)) 
            return BadRequest("User role is missing.");

        List<DTO.Advertisement> adv;

        if (role == UserType.Student.ToString())
            adv = recommendation.GetAdvertisementsForStudent(userId).Select(ad => ad.ToDto()).ToList();
        else if (role == UserType.Company.ToString())
            adv = recommendation.GetAdvertisementsOfCompany(userId).Select(ad => ad.ToDto()).ToList();
        else
            return BadRequest("Invalid user role.");

        return Ok(adv);
    }

    [HttpPost("advertisements")]
    [Authorize]
    [SwaggerOperation(Summary = "Create a new advertisement", Description = "Create a new advertisement for a company. The advertisement is created based on the data provided in the request body.")]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult CreateAdvertisement([FromBody] DTO.AdvertisementRegistration advertisement) {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Company.ToString())
            return BadRequest("Invalid role\n");
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        // Add advertisement data to DB
        if (recommendation.CreateAdvertisement(userId, advertisement))
            return Ok("Advertisement registered\n");
        else
            return StatusCode(500, "Internal server error\n");
    }

    [HttpGet("advertisements/{id}")]
    [Authorize]
    [SwaggerOperation(Summary = "Get a specific advertisement", Description = "Get a specific advertisement by its ID.")]
    [ProducesResponseType(501)]
    public IActionResult GetAdvertisement(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet("candidates")]
    [SwaggerOperation(Summary = "Get recommended students for specific advertisements", Description = "Get candidates for specific advertisements. The candidates are students that have been suggested by the recommendation system based on their skills and the advertisement requirements.")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetCandidates() {
        return StatusCode(501, "Feature not yet implemented\n");
    }
    
    [HttpPost("suggestion")]
    [Authorize]
    [SwaggerOperation(Summary = "Create a new suggestion by a company for one student", Description = "Create a new suggestion for a student. The suggestion is added to the database and the student is notified. One company can decide to suggest a student for a specific advertisement.")]
    [ProducesResponseType(501)]
    public IActionResult CreateSuggestion() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

}

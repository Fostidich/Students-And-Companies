using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/recommendation")]
public class RecommendationController : ControllerBase {

    private readonly IRecommendationService recommendation;

    public RecommendationController(IRecommendationService service) {
        this.recommendation = service;
    }

    [HttpGet("advertisements")]
    [Authorize]
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
            adv = recommendation.GetAdvertisementsForStudent(userId).Select(adv => adv.ToDto()).ToList();
        else if (role == UserType.Company.ToString())
            adv = recommendation.GetAdvertisementsOfCompany(userId).Select(adv => adv.ToDto()).ToList();
        else
            return BadRequest("Invalid user role.");

        return Ok(adv);
    }

    [HttpPost("advertisements")]
    [Authorize]
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

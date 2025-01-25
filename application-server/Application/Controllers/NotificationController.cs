using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/notification")]
public class NotificationController : ControllerBase {

    private readonly INotificationService notification;

    public NotificationController(INotificationService service) {
        this.notification = service;
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Get notifications for a student", Description = "Get notifications for a student. The notifications are filtered based on the student ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetNotifications() {
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        List<StudentNotifications> checkNotifications = notification.GetStudentNotifications(userId);
        
        if (checkNotifications == null)
            return StatusCode(500, "Internal server error\n");
        
        if (checkNotifications.Count == 0)
            return NotFound("You don't have notifications\n");
        
        List<DTO.StudentNotifications> notifications = checkNotifications.Select(not => not.ToDto()).ToList();
        
        return Ok(notifications);
    }
    
    
    [HttpPost("delete/{notificationId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Delete a notification for a student", Description = "Delete a notification for a student. The notification is deleted based on the notification ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteNotification(int notificationId) {
        
        if (notificationId <= 0) return BadRequest("Invalid id\n");
        
        // Check role
        string role = User.FindFirst(ClaimTypes.Role).Value;
        if (role != UserType.Student.ToString())
            return BadRequest("Invalid role\n");
        
        // Get user ID from authentication token
        string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId = Convert.ToInt32(userIdStr);
        
        bool result = notification.DeleteNotification(notificationId, userId);
        
        if (!result)
            return NotFound("You don't have this notification\n");
        
        return Ok("Notification deleted\n");
    }

}

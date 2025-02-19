using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/notification")]
public class NotificationController : ControllerBase
{

    private readonly INotificationService notification;

    public NotificationController(INotificationService service)
    {
        this.notification = service;
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Get notifications of the student", Description = "Return the notifications of the student with the provided ID.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetNotifications()
    {
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
            return NotFound("No notification found\n");

        List<DTO.StudentNotifications> notifications = checkNotifications.Select(not => not.ToDto()).ToList();

        return Ok(notifications);
    }

    [HttpPost("delete/{notificationId}")]
    [Authorize]
    [SwaggerOperation(Summary = "Delete the notification of the student", Description = "The notification with the provided ID is deleted (marked as read) from the student notification panel.")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult DeleteNotification(int notificationId)
    {
        // Check id validity
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
            return NotFound("No notification found\n");

        return Ok("Notification deleted\n");
    }

}

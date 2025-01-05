using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/notification")]
public class NotificationController : ControllerBase {

    private readonly INotificationService notification;

    public NotificationController(INotificationService service) {
        this.notification = service;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetNotifications() {
        return StatusCode(501, "Feature not yet implemented\n");
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(501)]
    public IActionResult GetNotification(int id) {
        return StatusCode(501, "Feature not yet implemented\n");
    }

}

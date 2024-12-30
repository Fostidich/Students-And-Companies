using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/notification")]
public class NotificationController : ControllerBase {

    private readonly INotificationService notification;

    public NotificationController(INotificationService service) {
        this.notification = service;
    }

}

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase {

    private readonly IFeedbackService feedback;

    public FeedbackController(IFeedbackService service) {
        this.feedback = service;
    }

}

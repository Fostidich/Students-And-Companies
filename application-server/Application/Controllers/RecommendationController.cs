using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/recommendation")]
public class RecommendationController : ControllerBase {

    private readonly IRecommendationService recommendation;

    public RecommendationController(IRecommendationService service) {
        this.recommendation = service;
    }

}

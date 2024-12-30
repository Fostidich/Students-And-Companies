using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/internship")]
public class InternshipController : ControllerBase {

    private readonly IInternshipService internship;

    public InternshipController(IInternshipService service) {
        this.internship = service;
    }

}

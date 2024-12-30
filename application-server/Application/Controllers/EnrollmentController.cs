using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/enrollment")]
public class EnrollmentController : ControllerBase {

    private readonly IEnrollmentService enrollment;

    public EnrollmentController(IEnrollmentService service) {
        this.enrollment = service;
    }

}

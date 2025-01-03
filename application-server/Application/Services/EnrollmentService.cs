public class EnrollmentService : IEnrollmentService {

    private readonly IEnrollmentQueries queries;

    public EnrollmentService(IEnrollmentQueries queries) {
        this.queries = queries;
    }

}

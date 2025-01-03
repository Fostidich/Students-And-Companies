public class InternshipService : IInternshipService {

    private readonly IInternshipQueries queries;

    public InternshipService(IInternshipQueries queries) {
        this.queries = queries;
    }

}

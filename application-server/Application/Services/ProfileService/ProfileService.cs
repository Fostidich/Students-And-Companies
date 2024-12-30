public class ProfileService : IProfileService {

    private readonly IProfileQueries queries;

    public ProfileService(IProfileQueries queries) {
        this.queries = queries;
    }

}

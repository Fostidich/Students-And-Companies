public class RecommendationService : IRecommendationService {

    private readonly IRecommendationQueries queries;

    public RecommendationService(IRecommendationQueries queries) {
        this.queries = queries;
    }

}

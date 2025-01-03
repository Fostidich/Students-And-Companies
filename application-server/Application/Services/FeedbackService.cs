public class FeedbackService : IFeedbackService {

    private readonly IFeedbackQueries queries;

    public FeedbackService(IFeedbackQueries queries) {
        this.queries = queries;
    }

}

public class NotificationService : INotificationService {

    private readonly INotificationQueries queries;

    public NotificationService(INotificationQueries queries) {
        this.queries = queries;
    }

}

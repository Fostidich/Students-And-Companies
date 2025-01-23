using System.Collections.Generic;
using System.Linq;

public class NotificationService : INotificationService {

    private readonly INotificationQueries queries;

    public NotificationService(INotificationQueries queries) {
        this.queries = queries;
    }
    
    public List<StudentNotifications> GetStudentNotifications(int studentId) {
        // Get student notifications
        List<Entity.StudentNotifications> studentNotifications = queries.GetStudentNotifications(studentId);
        
        // If exceptions occur, return null
        if(studentNotifications == null) {
            return null;
        }
        
        // Convert Entity.StudentNotifications to StudentNotifications
        List<StudentNotifications> notifications = studentNotifications
            .Select(notification => new StudentNotifications(notification))
            .ToList();
        
        return notifications;
    }

}

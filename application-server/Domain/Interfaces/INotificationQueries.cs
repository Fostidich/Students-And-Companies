using System.Collections.Generic;

public interface INotificationQueries {
    List<Entity.StudentNotifications> GetStudentNotifications(int studentId);
    bool DeleteNotification(int notificationId, int studentId);
    
}

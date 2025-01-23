using System.Collections.Generic;

public interface INotificationService {
    
    List<StudentNotifications> GetStudentNotifications(int studentId);

}

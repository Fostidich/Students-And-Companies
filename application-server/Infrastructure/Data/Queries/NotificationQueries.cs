using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

public class NotificationQueries : INotificationQueries
{

    private readonly IDataService dataService;
    private readonly IInternshipQueries internshipQueries;

    public NotificationQueries(IDataService dataService, IInternshipQueries internshipQueries)
    {
        this.dataService = dataService;
        this.internshipQueries = internshipQueries;
    }

    public List<Entity.StudentNotifications> GetStudentNotifications(int studentId)
    {
        try
        {
            string query = @"
                SELECT *
                FROM student_notifications
                WHERE student_id = @StudentId;
            ";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@StudentId", studentId);

            using var reader = command.ExecuteReader();

            var studentNotifications = dataService.MapToStudentNotifications(reader);
            return studentNotifications;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool DeleteNotification(int notificationId, int studentId)
    {
        try
        {
            string query = @"
                DELETE FROM student_notifications
                WHERE student_notification_id = @NotificationId
                AND student_id = @StudentId;
            ";

            using var db_connection = dataService.GetConnection();
            using var command = new MySqlCommand(query, db_connection);

            command.Parameters.AddWithValue("@NotificationId", notificationId);
            command.Parameters.AddWithValue("@StudentId", studentId);

            int result = command.ExecuteNonQuery();

            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

}

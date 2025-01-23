using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

public class NotificationQueries : INotificationQueries {
    
    private readonly IDataService dataService;

    public NotificationQueries(IDataService dataService) {
        this.dataService = dataService;
    }
    
    public List<Entity.StudentNotifications> GetStudentNotifications(int studentId) {
        try {
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
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

}

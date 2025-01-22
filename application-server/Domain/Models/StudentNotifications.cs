namespace ApplicationServer.Domain.Models;

public class StudentNotifications {
        
        public int StudentNotificationId { get; set; }
        public int StudentId { get; set; }
        public int AdvertisementId { get; set; }
        public char Type { get; set; }
        
        public StudentNotifications (Entity.StudentNotifications studentNotifications) {
            StudentNotificationId = studentNotifications.StudentNotificationId;
            StudentId = studentNotifications.StudentId;
            AdvertisementId = studentNotifications.AdvertisementId;
            Type = studentNotifications.Type;
        }
        
        public DTO.StudentNotifications ToDto() {
            return new DTO.StudentNotifications {
                StudentNotificationId = StudentNotificationId,
                StudentId = StudentId,
                AdvertisementId = AdvertisementId,
                Type = Type,
            };
        }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Entity {

    public class CompanyNotifications {

        public CompanyNotifications() { }

        public CompanyNotifications(IDataReader reader) {
            CompanyNotificationId = Convert.ToInt32(reader["company_notification_id"]);
            CompanyId = Convert.ToInt32(reader["company_id"]);
            StudentId = Convert.ToInt32(reader["student_id"]);
            AdvertisementId = Convert.ToInt32(reader["advertisement_id"]);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyNotificationId { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

        
        // Navigation properties
        
        public Entity.Company Company { get; set; }
        public Entity.Student Student { get; set; }
        public Entity.Advertisement Advertisement { get; set; }
        
    }
}
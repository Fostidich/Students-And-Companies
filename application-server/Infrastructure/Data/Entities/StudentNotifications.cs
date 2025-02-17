using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Entity
{

    public class StudentNotifications
    {

        public StudentNotifications() { }

        public StudentNotifications(IDataReader reader)
        {
            StudentNotificationId = Convert.ToInt32(reader["student_notification_id"]);
            StudentId = Convert.ToInt32(reader["student_id"]);
            AdvertisementId = Convert.ToInt32(reader["advertisement_id"]);
            Type = reader["type"].ToString();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentNotificationId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Column(TypeName = "enum('INVITED', 'RECOMMENDED', 'ACCEPTED', 'REJECTED')")]
        public string Type { get; set; }


        // Navigation properties

        public Entity.Student Student { get; set; }
        public Entity.Advertisement Advertisement { get; set; }

    }
}

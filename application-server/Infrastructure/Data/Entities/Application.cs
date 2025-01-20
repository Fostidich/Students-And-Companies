using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class Application {

        public Application() { }

        public Application(IDataReader reader) {
            ApplicationId = Convert.ToInt32(reader["application_id"]);
            StudentId = Convert.ToInt32(reader["student_id"]);
            AdvertisementId = Convert.ToInt32(reader["advertisement_id"]);
            CreatedAt = DateTime.Parse(reader["created_at"].ToString());
            Status = reader["status"].ToString();
            Questionnaire = reader["questionnaire"].ToString();
            ProposedStart = reader["proposed_start"] != DBNull.Value
                ? DateTime.Parse(reader["proposed_start"].ToString())
                : default;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Column(TypeName = "enum('REQUESTED', 'ACCEPTED', 'STARTED')")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public String Questionnaire { get; set; }

        public DateTime ProposedStart { get; set; }


        // Navigation properties

        public Student Student { get; set; }
        public Advertisement Advertisement { get; set; }

    }

}

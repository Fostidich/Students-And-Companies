using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Entity {

    public class Application {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; } // Primary key for the Application table

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Student")]
        public int StudentId { get; set; } // Foreign key to the Student table

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; } // Foreign key to the Advertisement table

        [Required(ErrorMessage = "Field is required")]
        public DateTime CreatedAt { get; set; } // Timestamp for when the application was created

        [Required(ErrorMessage = "Field is required")]
        [Column(TypeName = "enum('REQUESTED', 'ACCEPTED', 'STARTED')")]
        public string Status { get; set; } // Status of the application

        [Required(ErrorMessage = "Field is required")]
        public String Questionnaire { get; set; } // Filled in questionnaire from the student

        public DateTime? ProposedStart { get; set; } // Nullable: Only set if the company accepts the application

        
        // Navigation properties
        public Student Student { get; set; }
        public Advertisement Advertisement { get; set; }
    }
}
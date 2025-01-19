using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class Application {

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

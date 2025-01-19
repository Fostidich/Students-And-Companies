using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class Internship {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InternshipId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public DateTime EndDate { get; set; }


        // Navigation properties

        public Student Student { get; set; }
        public Company Company { get; set; }
        public Advertisement Advertisement { get; set; }
        public Feedback Feedback { get; set; }

    }

}

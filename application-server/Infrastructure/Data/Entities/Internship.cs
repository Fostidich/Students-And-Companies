using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Entity {

    public class Internship {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InternshipId { get; set; } // Primary key for the Internship table

        [Required(ErrorMessage = "Field is required")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp for when the internship was created

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Student")]
        public int StudentId { get; set; } // Foreign key to the Student table

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; } // Foreign key to the Company table

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; } // Foreign key to the Advertisement table

        [Required(ErrorMessage = "Field is required")]
        public DateTime StartDate { get; set; } // Start date of the internship

        [Required(ErrorMessage = "Field is required")]
        public DateTime EndDate { get; set; } // End date of the internship

        // Navigation properties
        public Student Student { get; set; }
        public Company Company { get; set; }
        public Advertisement Advertisement { get; set; }
        
        public Feedback Feedback { get; set; }
        
    }
}
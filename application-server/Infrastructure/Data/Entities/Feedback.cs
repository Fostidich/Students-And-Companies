using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class Feedback {

        [Key]
        [ForeignKey("Internship")]
        public int InternshipId { get; set; } // Foreign key to the Internship table

        [Required(ErrorMessage = "Field is required")]
        [Range(1, 10, ErrorMessage = "StudentRating must be between 1 and 10.")]
        public int StudentRating { get; set; } // Rating provided by the student

        [Required(ErrorMessage = "Field is required")]
        [Range(1, 10, ErrorMessage = "CompanyRating must be between 1 and 10.")]
        public int CompanyRating { get; set; } // Rating provided by the company

        [MaxLength(1024, ErrorMessage = "StudentComment cannot exceed 1024 characters.")]
        public string StudentComment { get; set; } // Comment from the student

        [MaxLength(1024, ErrorMessage = "CompanyComment cannot exceed 1024 characters.")]
        public string CompanyComment { get; set; } // Comment from the company

        // Navigation property
        public Internship Internship { get; set; }
    }
}
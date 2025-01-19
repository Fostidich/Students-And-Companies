using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class Feedback {

        [Key]
        [ForeignKey("Internship")]
        public int InternshipId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
        public int StudentRating { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
        public int CompanyRating { get; set; }

        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string StudentComment { get; set; }

        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string CompanyComment { get; set; }


        // Navigation property

        public Internship Internship { get; set; }

    }

}

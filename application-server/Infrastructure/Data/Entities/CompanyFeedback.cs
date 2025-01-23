using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Entity {
    
    public class CompanyFeedback {
        
        public CompanyFeedback() { }

        public CompanyFeedback(IDataReader reader) {
            InternshipId = Convert.ToInt32(reader["internship_id"]);
            Rating = Convert.ToInt32(reader["rating"]);
            Comment = reader["comment"].ToString();
        }

        [Key]
        [ForeignKey("Internship")]
        public int InternshipId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string Comment { get; set; }


        // Navigation property

        public Entity.Internship Internship { get; set; }
        
    
    }
}


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Collections.Generic;

namespace Entity {

    public class Advertisement {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvertisementId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public DateTime CreatedAt { get; set; } 

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 months.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Spots must be at least 1.")]
        public int Spots { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Available spots cannot be negative.")]
        public int Available { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public bool Open { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public String Questionnaire { get; set; }

        
        // Navigation properties
        public Company Company { get; set; }
        
        public ICollection<Entity.AdvertisementSkills> AdvertisementSkills { get; set; }
        public ICollection<Entity.Application> Applications { get; set; }
        public ICollection<Entity.Internship> Internships { get; set; } 
    }
}
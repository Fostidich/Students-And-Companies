using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Entity {

    public class Advertisement {

        public Advertisement() { }

        public Advertisement(IDataReader reader) {
            AdvertisementId = Convert.ToInt32(reader["advertisement_id"]);
            Name = reader["name"].ToString();
            CreatedAt = DateTime.Parse(reader["created_at"].ToString());
            CompanyId = Convert.ToInt32(reader["company_id"]);
            Description = reader["description"].ToString();
            Duration = Convert.ToInt32(reader["duration"]);
            Spots = Convert.ToInt32(reader["spots"]);
            Available = Convert.ToInt32(reader["available"]);
            Open = Convert.ToBoolean(reader["open"]);
            Questionnaire = reader["questionnaire"].ToString();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvertisementId { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(1, 12, ErrorMessage = "Duration must be between 1 and 12 months")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Spots must be a positive number")]
        public int Spots { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Available spots must be a positive number")]
        public int Available { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public bool Open { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public string Questionnaire { get; set; }


        // Navigation properties

        public Entity.Company Company { get; set; }

        public ICollection<Entity.AdvertisementSkills> AdvertisementSkills { get; set; }
        public ICollection<Entity.Application> Applications { get; set; }
        public ICollection<Entity.Internship> Internships { get; set; }
        public ICollection<Entity.StudentNotifications> StudentNotifications { get; set; }

    }

}

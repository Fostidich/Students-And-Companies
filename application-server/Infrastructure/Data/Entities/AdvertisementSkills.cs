using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {
    public class AdvertisementSkills {
        [Key, Column(Order = 0)] // First part of the composite primary key
        [ForeignKey("Advertisement")] // Foreign key to Advertisement
        public int AdvertisementId { get; set; }

        [Key, Column(Order = 1)] // Second part of the composite primary key
        [ForeignKey("Skill")] // Foreign key to Skill
        public int SkillId { get; set; }

        
        // Navigation properties
        public Advertisement Advertisement { get; set; }
        public Skill Skill { get; set; }
    }
}
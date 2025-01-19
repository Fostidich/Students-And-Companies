using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {
    public class StudentSkills {
        
        [Key, Column(Order = 0)] 
        [ForeignKey("Student")] 
        public int StudentId { get; set; }

        [Key, Column(Order = 1)] 
        [ForeignKey("Skill")] 
        public int SkillId { get; set; }

        // Proprietà di navigazione
        public Student Student { get; set; }
        public Skill Skill { get; set; }
    }
}
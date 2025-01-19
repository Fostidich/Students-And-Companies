using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class StudentSkills {
        
        public StudentSkills(IDataReader reader) {
            StudentId = Convert.ToInt32(reader["student_id"]);
            SkillId = Convert.ToInt32(reader["skill_id"]);
        }

        [Key, Column(Order = 0)]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Skill")]
        public int SkillId { get; set; }


        // Navigation properties

        public Student Student { get; set; }
        public Skill Skill { get; set; }

    }

}

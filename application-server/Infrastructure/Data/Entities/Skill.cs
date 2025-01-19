using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Entity {

    public class Skill {
        
        public Skill(IDataReader reader) {
            SkillId = Convert.ToInt32(reader["skill_id"]);
            Name = reader["name"].ToString();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        public string Name { get; set; }


        // Navigation properties

        public ICollection<Entity.StudentSkills> StudentSkills { get; set; }
        public ICollection<Entity.AdvertisementSkills> AdvertisementSkills { get; set; }

    }

}

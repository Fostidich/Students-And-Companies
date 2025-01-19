using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class AdvertisementSkills {

        [Key, Column(Order = 0)]
        [ForeignKey("Advertisement")]
        public int AdvertisementId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Skill")]
        public int SkillId { get; set; }


        // Navigation properties

        public Advertisement Advertisement { get; set; }
        public Skill Skill { get; set; }

    }

}

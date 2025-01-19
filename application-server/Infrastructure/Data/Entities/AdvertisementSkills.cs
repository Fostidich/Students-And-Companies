using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System;

namespace Entity {

    public class AdvertisementSkills {

        public AdvertisementSkills() { }

        public AdvertisementSkills(IDataReader reader) {
            AdvertisementId = Convert.ToInt32(reader["advertisement_id"]);
            SkillId = Convert.ToInt32(reader["skill_id"]);
        }


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

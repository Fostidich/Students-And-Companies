using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class Student {

        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        [MaxLength(255)]
        public string CvFilePath { get; set; }


        // Navigation properties

        public User User { get; set; }

    }

}

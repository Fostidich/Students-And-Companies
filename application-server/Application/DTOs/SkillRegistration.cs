using System.ComponentModel.DataAnnotations;

namespace DTO
{

    public class SkillRegistration
    {

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value is too long")]
        public string Name { get; set; }

    }

}

using System.ComponentModel.DataAnnotations;

namespace DTO
{

    public class Feedback
    {

        [Required(ErrorMessage = "Field is required")]
        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string Comment { get; set; }
    }
}
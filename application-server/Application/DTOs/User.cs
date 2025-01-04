using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class User {

        [Required(ErrorMessage = "Field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be positive")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        [MinLength(4, ErrorMessage = "Value must be at least 4 characters long")]
        [RegularExpression(@"^(?!.*@.*\..*$)(\S+)$", ErrorMessage = "Value cannot be an email address nor contain spaces")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"^(Student|Company)$", ErrorMessage = "The value must be either 'Student' or 'Company'")]
        public string UserType { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [EmailAddress(ErrorMessage = "Value must be a valid email address")]
        public string Email { get; set; }

    }

}

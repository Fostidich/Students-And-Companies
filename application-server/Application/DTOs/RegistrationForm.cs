using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class RegistrationForm {

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        [MinLength(4, ErrorMessage = "Value must be at least 4 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Invalid characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        [MinLength(8, ErrorMessage = "Value must be at least 8 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"^(Student|Company)$", ErrorMessage = "The value must be either 'Student' or 'Company'")]
        public string UserType { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(50, ErrorMessage = "Value cannot be more than 50 characters long")]
        [EmailAddress(ErrorMessage = "Value must be a valid email address")]
        public string Email { get; set; }

    }

}

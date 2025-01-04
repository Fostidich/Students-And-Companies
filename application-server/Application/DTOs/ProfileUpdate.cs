using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class ProfileUpdate {

        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        [MinLength(4, ErrorMessage = "Value must be at least 4 characters long")]
        [RegularExpression(@"^(?!.*@.*\..*$)(\S+)$", ErrorMessage = "Value cannot be an email address nor contain spaces")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Value must be a valid email address")]
        public string Email { get; set; }

        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        [MinLength(8, ErrorMessage = "Value must be at least 8 characters long")]
        public string Password { get; set; }

    }

}

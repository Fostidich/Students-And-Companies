using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class Company {

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value is too long")]
        [MinLength(4, ErrorMessage = "Value is too short")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Invalid characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value is too long")]
        [EmailAddress(ErrorMessage = "Value must be a valid email address")]
        public string Email { get; set; }

        [MaxLength(1024, ErrorMessage = "Text is too long")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string Headquarters { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string FiscalCode { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string VatNumber { get; set; }

    }

}

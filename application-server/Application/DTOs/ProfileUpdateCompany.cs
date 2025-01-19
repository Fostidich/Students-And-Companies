using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class ProfileUpdateCompany {

        [MaxLength(32, ErrorMessage = "Value is too long")]
        [MinLength(4, ErrorMessage = "Value is too short")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Invalid characters")]
        public string Username { get; set; }

        [MaxLength(64, ErrorMessage = "Value is too long")]
        [EmailAddress(ErrorMessage = "Value must be a valid email address")]
        public string Email { get; set; }

        [MaxLength(32, ErrorMessage = "Value is too long")]
        [MinLength(8, ErrorMessage = "Value is too short")]
        public string Password { get; set; }

        [MaxLength(1024, ErrorMessage = "Text is too long")]
        public string Bio { get; set; }

        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string Headquarters { get; set; }

        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string FiscalCode { get; set; }

        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string VatNumber { get; set; }

    }

}

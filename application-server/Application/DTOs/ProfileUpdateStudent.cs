using System.ComponentModel.DataAnnotations;
using System;

namespace DTO {

    public class ProfileUpdateStudent {

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
        public string Name { get; set; }

        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string Surname { get; set; }

        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string University { get; set; }

        [MaxLength(64, ErrorMessage = "Text is too long")]
        public string CourseOfStudy { get; set; }

        [RegularExpression("^[mf]$", ErrorMessage = "Sex must be either 'm' or 'f'")]
        public char? Gender { get; set; }

        [Range(typeof(DateTime), "01/01/1920", "01/01/2020", ErrorMessage = "Date outside the valid range")]
        public DateTime? BirthDate { get; set; }

    }

}

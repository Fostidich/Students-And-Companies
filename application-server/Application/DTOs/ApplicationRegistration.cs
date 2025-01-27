using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class ApplicationRegistration {

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string QuestionnaireAnswer { get; set; }

    }

}

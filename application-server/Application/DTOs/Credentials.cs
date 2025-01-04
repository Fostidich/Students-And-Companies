using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class Credentials {

        [Required(ErrorMessage = "Field is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public string Password { get; set; }

    }

}

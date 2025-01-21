using System.ComponentModel.DataAnnotations;

namespace DTO {

    public class AdvertisementRegistration {

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [Range(1, 12, ErrorMessage = "Duration must be between 1 and 12 months")]
        public int Duration { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Spots must be a positive number")]
        public int Spots { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        public string Questionnaire { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity {

    public class User {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        [MinLength(4, ErrorMessage = "Value must be at least 4 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Invalid characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(50, ErrorMessage = "Value cannot be more than 50 characters long")]
        [EmailAddress(ErrorMessage = "Value must be a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [StringLength(24, ErrorMessage = "Value cannot be more than 24 characters long")]
        public string Salt { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [StringLength(44, ErrorMessage = "Value cannot be more than 44 characters long")]
        public string HashedPassword { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Column(TypeName = "enum('Student','Company')")]
        public string UserType { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

    }

}

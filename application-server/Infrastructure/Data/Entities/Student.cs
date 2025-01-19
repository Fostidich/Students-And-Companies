using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Entity {

    public class Student {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(50, ErrorMessage = "Value cannot be more than 50 characters long")]
        [EmailAddress(ErrorMessage = "Value must be a valid email address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        [MinLength(4, ErrorMessage = "Value must be at least 4 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Invalid characters")]
        public string UserName { get; set; }

		[Required(ErrorMessage = "Field is required")]
        [StringLength(24, ErrorMessage = "Value cannot be more than 24 characters long")]
        public string Salt { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [StringLength(44, ErrorMessage = "Value cannot be more than 44 characters long")]
        public string HashedPassword { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string University { get; set; }
        
		[Column(TypeName = "enum('M','W')")]
        public char Gender { get; set; }
        
		[Range(18, 120, ErrorMessage = "Age must be between 18 and 120.")]
        public int Age { get; set; }
        
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string CourseOfStudy { get; set; }
        
		[MaxLength(1024, ErrorMessage = "Bio cannot exceed 500 characters.")]
        public string Bio { get; set; }
        
        
        [MaxLength(255)]
        public string CvFilePath { get; set; }


		// Navigation properties
		public Internship Internship { get; set; }		
		public ICollection<Entity.StudentSkills> StudentSkills { get; set; }
		public ICollection<Entity.Application> Applications { get; set; }
		
    }

}

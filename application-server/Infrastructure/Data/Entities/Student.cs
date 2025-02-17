using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Entity
{

    public class Student
    {

        public Student() { }

        public Student(IDataReader reader)
        {
            StudentId = Convert.ToInt32(reader["student_id"]);
            CreatedAt = DateTime.Parse(reader["created_at"].ToString());
            Email = reader["email"].ToString();
            Username = reader["username"].ToString();
            Salt = reader["salt"].ToString();
            HashedPassword = reader["hashed_password"].ToString();
            Bio = reader["bio"] != DBNull.Value ? reader["bio"].ToString() : null;
            Name = reader["name"].ToString();
            Surname = reader["surname"].ToString();
            University = reader["university"].ToString();
            CourseOfStudy = reader["course_of_study"].ToString();
            Gender = reader["gender"].ToString()[0];
            BirthDate = DateTime.Parse(reader["birth_date"].ToString());
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        [EmailAddress(ErrorMessage = "Value must be a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(32, ErrorMessage = "Value cannot be more than 32 characters long")]
        [MinLength(4, ErrorMessage = "Value must be at least 4 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Invalid characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [StringLength(24, ErrorMessage = "Value cannot be more than 24 characters long")]
        public string Salt { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [StringLength(44, ErrorMessage = "Value cannot be more than 44 characters long")]
        public string HashedPassword { get; set; }

        [MaxLength(1024, ErrorMessage = "Value cannot be more than 1024 characters long")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string University { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string CourseOfStudy { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Column(TypeName = "enum('m','f')")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Range(typeof(DateTime), "01/01/1920", "01/01/2020", ErrorMessage = "Date outside the valid range")]
        public DateTime BirthDate { get; set; }


        // Navigation properties

        public ICollection<Entity.Internship> Internships { get; set; }
        public ICollection<Entity.StudentSkills> StudentSkills { get; set; }
        public ICollection<Entity.Application> Applications { get; set; }
        public ICollection<Entity.StudentNotifications> StudentNotifications { get; set; }

    }

}

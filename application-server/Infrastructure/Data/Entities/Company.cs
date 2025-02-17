using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Data;
using System;

namespace Entity
{

    public class Company
    {

        public Company() { }

        public Company(IDataReader reader)
        {
            CompanyId = Convert.ToInt32(reader["company_id"]);
            CreatedAt = DateTime.Parse(reader["created_at"].ToString());
            Email = reader["email"].ToString();
            Username = reader["username"].ToString();
            Salt = reader["salt"].ToString();
            HashedPassword = reader["hashed_password"].ToString();
            Bio = reader["bio"] != DBNull.Value ? reader["bio"].ToString() : null;
            Headquarter = reader["headquarter"].ToString();
            FiscalCode = reader["fiscal_code"].ToString();
            VatNumber = reader["vat_number"].ToString();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

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
        public string Headquarter { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string FiscalCode { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [MaxLength(64, ErrorMessage = "Value cannot be more than 64 characters long")]
        public string VatNumber { get; set; }


        // Navigation properties

        public ICollection<Entity.Advertisement> Advertisements { get; set; }
        public ICollection<Entity.Internship> Internships { get; set; }

    }

}

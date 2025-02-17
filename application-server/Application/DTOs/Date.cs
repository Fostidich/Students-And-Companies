using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{

    public class Date
    {

        [Required(ErrorMessage = "Field is required")]
        public DateTime DateTime { get; set; }

    }

}

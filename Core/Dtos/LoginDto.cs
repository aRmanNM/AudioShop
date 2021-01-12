using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class LoginDto
    {
        // public string Email { get; set; }
        // public string Password { get; set; }

        [Required(ErrorMessage = "وارد کردن شماره تماس ضروریست")]
        [RegularExpression("^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{4}$")]
        public string PhoneNumber { get; set; }
    }
}

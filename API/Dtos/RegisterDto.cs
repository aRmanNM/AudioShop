﻿using System.ComponentModel.DataAnnotations;
using API.Helpers;
using API.Models;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public string SalespersonCouponCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public short Age { get; set; }
        public Gender Gender { get; set; }
        public EmploymentStatus Employed { get; set; }
    }
}

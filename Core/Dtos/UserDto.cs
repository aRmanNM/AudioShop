using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class UserDto
    {
        public string Token { get; set; }
        public bool HasPhoneNumber { get; set; }
    }
}

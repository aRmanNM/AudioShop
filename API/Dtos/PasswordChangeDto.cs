﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class PasswordChangeDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

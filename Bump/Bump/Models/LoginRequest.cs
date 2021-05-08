using System;
using System.Collections.Generic;
using System.Text;

namespace Bump.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRememberMe { get; set; }
    }
}

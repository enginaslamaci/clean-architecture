using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.DTOs.Account.Request
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}

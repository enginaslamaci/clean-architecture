using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.DTOs.Account.Request
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "The password length must bu greather six digit.")]
        public string? Password { get; set; }

        //[Required]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string? ConfirmPassword { get; set; }
    }
}

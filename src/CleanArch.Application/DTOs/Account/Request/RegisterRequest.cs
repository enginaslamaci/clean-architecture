using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.DTOs.Account.Request
{
    public class RegistrationRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //[Required]
        //[MinLength(6)]
        //public string UserName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        //[Required]
        //[MinLength(6)]
        //public string ConfirmPassword { get; set; }


        /// <summary>
        /// Sözleşmeler
        /// </summary>
        //[Required]
        //public Confirmation Confirmations { get; set; }
    }
}

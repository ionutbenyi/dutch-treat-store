using System;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public Boolean RememberMe { get; set; }
    }
}

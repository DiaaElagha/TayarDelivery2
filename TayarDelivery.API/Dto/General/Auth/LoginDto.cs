using System;
using System.ComponentModel.DataAnnotations;

namespace TayarDelivery.API.Dto.Auth
{
    public class LoginDto
    {

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FcmToken { get; set; }

    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace TayarDelivery.API.Dto.Auth
{
    public class VerifySMSCodeDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public int Code { get; set; }
        [Required]
        public int CodeType { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace TayarDelivery.API.Dto.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Please Enter Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Enter UserName")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Enter Valid Format Email")]
        public string Email { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please Enter Mobile Number")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter a 10-digit number")]
        public string MobileNumber1 { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter a 10-digit number")]
        public string MobileNumber2 { get; set; }

        [Required(ErrorMessage = "Please Enter Your Area Name")]
        public int AreaId { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        [Required]
        public int PriceTypeId { get; set; }
        [Required]
        public string FcmToken { get; set; }
    }
}

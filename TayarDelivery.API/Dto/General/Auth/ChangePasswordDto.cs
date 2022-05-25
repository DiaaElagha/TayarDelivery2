using System.ComponentModel.DataAnnotations;

namespace TayarDelivery.API.Dto.Auth
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}

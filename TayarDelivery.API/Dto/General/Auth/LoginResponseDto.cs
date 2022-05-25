using System;
using TayarDelivery.API.Dto.General.Auth;

namespace TayarDelivery.API.Dto.Auth
{
    public class LoginResponseDto
    {
        public UserProfileDto ProfileData { get; set; }
        public UserProfileSettingsDto ProfileSettings { get; set; }
        public string AccessToken { get; set; }
    }
}

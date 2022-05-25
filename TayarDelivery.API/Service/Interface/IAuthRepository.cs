using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.API.Dto.Auth;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.API.Service.Interface
{
    public interface IAuthRepository
    {
        Task<SignInResult> Login(LoginDto login);
        Task<IdentityResult> SignUp(ApplicationUser user, string password, string userTypeName);
        Task<string> CreateAccess(ApplicationUser user);
    }
}

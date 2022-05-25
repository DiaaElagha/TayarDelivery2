using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TayarDelivery.Data.Data;

namespace TayarDelivery.Areas.Admin.Controllers.Base
{
    [Authorize]
    public class ValidationController : Controller
    {
        private ApplicationDbContext _Context;
        public ValidationController(ApplicationDbContext context)
        {
            _Context = context;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyUserName(string UserName)
        {
            if (IsExists(UserName))
            {
                return Json($"{UserName} موجود بالفعل");
            }
            return Json(true);
        }

        public bool IsExists(string UserName)
        {
            return _Context.Users.Any(x => x.UserName.Trim().Equals(UserName.Trim()));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected String UserId;
        protected String UserName;
        protected String UserType;

        public BaseController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                base.OnActionExecuting(filterContext);
                try
                {
                    UserId = _userManager.GetUserId(HttpContext.User);
                    var user = _userManager.Users.Include(x => x.UserType).SingleOrDefault(x => x.Id.Equals(UserId));
                    UserName = user.UserName;
                    UserType = user.UserType.TitlePrograming;
                    ViewBag.UserId = UserId;
                    ViewBag.UserType = user.UserType.TitlePrograming;
                    ViewBag.FullName = user.FullName;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}

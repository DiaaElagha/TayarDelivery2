using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TayarDelivery.Data.Data;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace TayarDelivery.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Route("Admin/[controller]/[action]/{id?}")]
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly IMapper _mapper;
        protected String UserId;
        protected String UserName;
        protected String UserType;
        protected RouteDataValues RouteDataValues = new RouteDataValues();

        public BaseController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ActionName.ToString().ToLower();
            var controllerName = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ControllerName.ToString().ToLower();
            RouteDataValues.ActionName = actionName;
            RouteDataValues.ControllerName = controllerName;
            if (User.Identity.IsAuthenticated)
            {
                base.OnActionExecuting(filterContext);
                try
                {
                    UserId = _userManager.GetUserId(HttpContext.User);
                    var user = _userManager.Users.Include(x => x.UserType).Include(x => x.Area).SingleOrDefault(x => x.Id.Equals(UserId));
                    UserName = user.UserName;
                    UserType = user.UserType.TitlePrograming;
                    ViewBag.UserId = UserId;
                    ViewBag.UserAreaId = user.Area != null ? user.Area.Id : 0;
                    ViewBag.UserTypeEN = user.UserType.TitlePrograming;
                    ViewBag.UserTypeAR = user.UserType.TitleView;
                    ViewBag.FullName = user.FullName;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public ApplicationUser GetUser() 
        {
            var user = _userManager.Users.Include(x => x.UserType).Include(x => x.Area).Include(x => x.PriceType).SingleOrDefault(x => x.Id.Equals(UserId));
            return user;
        }

        public static string GetIpAddress()  // Get IP Address
        {
            string ip = "";
            IPHostEntry ipEntry = Dns.GetHostEntry(GetCompCode());
            IPAddress[] addr = ipEntry.AddressList;
            ip = addr[2].ToString();
            return ip;
        }

        public static string GetCompCode()  // Get Computer Name
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();
            return strHostName;
        }

        public static string GetMacAddress()  // Get Mac Address (Physical Address)
        {
            String firstMacAddress = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();
            return firstMacAddress;
        }


    }
}

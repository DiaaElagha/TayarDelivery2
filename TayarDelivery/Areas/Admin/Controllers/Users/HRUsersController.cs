using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Helper;
using TayarDelivery.Models.ViewModel.Auth;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.Areas.Admin.Controllers.Users
{
    public class HRUsersController : BaseController
    {
        protected readonly IBaseRepository<UserType> _userTypeRepository;
        protected readonly IBaseRepository<RolesUser> _rolesUserRepository;
        protected readonly IBaseRepository<Role> _roleRepository;
        public HRUsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IBaseRepository<UserType> userTypeRepository,
            IBaseRepository<RolesUser> rolesUserRepository,
            IBaseRepository<Role> roleRepository) 
            : base(userManager, roleManager, mapper)
        {
            _userTypeRepository = userTypeRepository;
            _rolesUserRepository = rolesUserRepository;
            _roleRepository = roleRepository;
        }
        public IActionResult Index()
        {
            ViewData["listUserTypes"] = new SelectList(_userTypeRepository.GetAll().Result, "Id", "TitleView");
            return View();
        }

        [HttpPost]
        public JsonResult AjaxDataHRUsers([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);
            string jsonString = data.ToString();
            JObject ss = JObject.Parse(jsonString);
            int userTypeId = (int)ss["UserTypeId"];

            var query = _userManager.Users.Include(x => x.UserType).Include(x => x.Area).Include(x => x.PriceType).Where(
                x => (x.UserTypeID == userTypeId || userTypeId == -1) && (d.SearchKey == null
                    || x.FullName.Contains(d.SearchKey)
                    || x.Email.Contains(d.SearchKey)
                    || x.MobileNumber1.Contains(d.SearchKey)
                    || x.MobileNumber2.Contains(d.SearchKey)
                    || x.Area.Name.Contains(d.SearchKey)
                    || x.PriceType.Name.Contains(d.SearchKey)
                    || x.UserName.Contains(d.SearchKey))).OrderByDescending(x => x.CreateAt).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.UserName,
                x.FullName,
                x.Email,
                Mobile1 = String.IsNullOrEmpty(x.MobileNumber1) ? "-" : x.MobileNumber1,
                Mobile2 = String.IsNullOrEmpty(x.MobileNumber2) ? "" : x.MobileNumber2,
                x.IsActive,
                AreaName = x.Area != null ? x.Area.Name : "-",
                PriceTypeName = x.PriceType != null ? x.PriceType.Name : "-",
                createAt = x.CreateAt.Value.ToString("MM/dd/yyyy"),
            }).Skip(d.Start).Take(d.Length).ToList();
            var result =
               new
               {
                   draw = d.Draw,
                   recordsTotal = totalCount,
                   recordsFiltered = totalCount,
                   data = items
               };
            return Json(result);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> UserRoles(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            return View(await UserRolesDetils(id));
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRoles(string id, UserRolesVM model)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var listRole = await _rolesUserRepository.Get().Where(x => x.UserId.Equals(id)).ToListAsync();
                if (listRole != null)
                {
                    if (listRole.Count() != 0)
                    {
                        foreach (var item in listRole)
                        {
                            await _rolesUserRepository.DeleteAsync(item);
                        }
                    }
                }

                var newRolesList = model.RolesId;
                if (newRolesList != null)
                {
                    if (newRolesList.Count() != 0)
                    {
                        foreach (var item in newRolesList)
                        {
                            await _rolesUserRepository.AddAsync(new RolesUser { RoleId = item, UserId = id });
                        }
                    }
                }

                return Content(ShowMessage.AddSuccessResult(), "application/json");
            }
            return View(await UserRolesDetils(id));
        }

        private async Task<UserRolesVM> UserRolesDetils(string id)
        {
            var userItem = await _userManager.FindByIdAsync(id);
            var listRole = await _rolesUserRepository.Get().Where(x => x.UserId.Equals(id)).Select(x => x.RoleId).ToArrayAsync();
            if (listRole == null)
            {
                return null;
            }
            UserRolesVM userRolesVM = new UserRolesVM
            {
                RoleList = new SelectList(_roleRepository.GetAll().Result, "Id", "Title"),
                RolesId = listRole,
                User = userItem
            };
            return userRolesVM;
        }


    }
}

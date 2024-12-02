﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TayarDelivery.Data.StaticModel;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.LookUp;
using TayarDelivery.Helper;
using TayarDelivery.Models.ViewModel.Users;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.Areas.Admin.Controllers.Users
{
    [Authorize(Roles = "administrator")]
    public class TraderController : BaseController
    {
        protected readonly IBaseRepository<Area> _areaRepository;
        protected readonly IBaseRepository<PriceType> _priceTypeRepository;
        protected readonly IBaseRepository<UserType> _userTypeRepository;
        public TraderController(
            IBaseRepository<Area> areaRepository,
            IBaseRepository<PriceType> priceTypeRepository,
            IBaseRepository<UserType> userTypeRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._areaRepository = areaRepository;
            this._priceTypeRepository = priceTypeRepository;
            this._userTypeRepository = userTypeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _userManager.Users.Include(x => x.UserType).Include(x => x.Area).Include(x => x.PriceType).Where(
                x => x.UserType.TitlePrograming.Equals(UserTypeValues.TRADER) && (d.SearchKey == null
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
                AreaName = x.Area.Name,
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


        // GET: Admin/Area/Create
        public IActionResult Create()
        {
            ViewData["AreaList"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["PriceTypeList"] = new SelectList(_priceTypeRepository.GetAll().Result, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Password, TraderVM model)
        {
            if (ModelState.IsValid)
            {
                var newEntity = _mapper.Map<ApplicationUser>(model);
                var userTypeItem = await _userTypeRepository.FindSingle(c => c.TitlePrograming.Equals(UserTypeValues.TRADER));
                newEntity.UserTypeID = userTypeItem.Id;
                if (String.IsNullOrEmpty(Password))
                {
                    Password = "Admin123$";
                }
                var result = await _userManager.CreateAsync(newEntity, Password);
                var result2 = await _userManager.AddToRoleAsync(newEntity, UserTypeValues.TRADER);
                if (result.Succeeded && result2.Succeeded)
                    if (result.Succeeded)
                    return Content(ShowMessage.AddSuccessResult(), "application/json");
                return Content(ShowMessage.FailedResult(), "application/json");
            }
            ViewData["AreaList"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["PriceTypeList"] = new SelectList(_priceTypeRepository.GetAll().Result, "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var model = await _userManager.FindByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            var newEntity = _mapper.Map<TraderVM>(model);
            ViewData["AreaList"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["PriceTypeList"] = new SelectList(_priceTypeRepository.GetAll().Result, "Id", "Name");
            return View(newEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string Password, TraderVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _userManager.FindByIdAsync(id);
                    PropertyCopy.Copy(model, baseEntoty);
                    var result = await _userManager.UpdateAsync(baseEntoty);
                    if (result.Succeeded)
                    {
                        if (!String.IsNullOrEmpty(Password))
                        {
                            var resultRemovePassword = await _userManager.RemovePasswordAsync(baseEntoty);
                            //  Add a user password only if one does not already exist
                            var resultAddPassword = await _userManager.AddPasswordAsync(baseEntoty, Password);
                        }
                        return Content(ShowMessage.EditSuccessResult(), "application/json");
                    }
                    return Content(ShowMessage.FailedResult(), "application/json");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["AreaList"] = new SelectList(_areaRepository.GetAll().Result, "Id", "Name");
            ViewData["PriceTypeList"] = new SelectList(_priceTypeRepository.GetAll().Result, "Id", "Name");
            return View();
        }

        // GET: Admin/Administrator/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _userManager.FindByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(model);
            if (result != null)
                return Content(ShowMessage.DeleteSuccessResult(), "application/json");
            return Content(ShowMessage.FailedResult(), "application/json");
        }

        private bool UserExists(string id)
        {
            return _userManager.Users.Any(e => e.Id.Equals(id));
        }

    }
}

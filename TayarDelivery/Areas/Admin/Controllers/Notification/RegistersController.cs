using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.Home;
using TayarDelivery.Entity.DTO;
using TayarDelivery.Helper;
using TayarDelivery.Models.ViewModel.Mail;
using TayarDelivery.Repository.Interfaces;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.Areas.Admin.Controllers.Notification
{
    public class RegistersController : BaseController
    {
        protected readonly IBaseRepository<RegisterTrader> _registerTraderRepository;
        public RegistersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IBaseRepository<RegisterTrader> registerTraderRepository)
            : base(userManager, roleManager, mapper)
        {
            _registerTraderRepository = registerTraderRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _registerTraderRepository.Filter(
                filter: x => (d.SearchKey == null 
                    || x.NameSocial.Contains(d.SearchKey) 
                    || x.City.Contains(d.SearchKey) 
                    || x.FullName.Contains(d.SearchKey)
                    || x.Email.Contains(d.SearchKey)
                    || x.Phone.Contains(d.SearchKey)),
                orderBy: x => x.OrderBy(x => x.Id)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.NameSocial,
                x.Email,
                x.FullName,
                x.City,
                x.Phone,
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

        public async Task<IActionResult> ViewContactUsMessege(int id)
        {
            var item = await _registerTraderRepository.Get(id);
            if (item == null)
                return View("ViewContactUsMessege","لا يوجد بيانات");
            return View("ViewContactUsMessege", item.NameSocial);
        }

    }
}

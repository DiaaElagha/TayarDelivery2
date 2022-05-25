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
    public class ContactUsController : BaseController
    {
        protected readonly IBaseRepository<ContactUs> _contactUsRepository;
        public ContactUsController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IBaseRepository<ContactUs> contactUsRepository)
            : base(userManager, roleManager, mapper)
        {
            _contactUsRepository = contactUsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _contactUsRepository.Filter(
                filter: x => (d.SearchKey == null 
                    || x.Messege.Contains(d.SearchKey) 
                    || x.Subject.Contains(d.SearchKey) 
                    || x.Name.Contains(d.SearchKey)
                    || x.Email.Contains(d.SearchKey)
                    || x.Phone.Contains(d.SearchKey)),
                orderBy: x => x.OrderBy(x => x.CreateAt)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.Subject,
                x.Name,
                x.Email,
                x.Phone,
                x.Messege,
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

        public async Task<IActionResult> ViewContactUsMessege(int id)
        {
            var item = await _contactUsRepository.Get(id);
            if (item == null)
                return View("ViewContactUsMessege","لا يوجد بيانات");
            return View("ViewContactUsMessege", item.Messege);
        }

    }
}

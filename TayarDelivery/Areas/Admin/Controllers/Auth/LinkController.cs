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
using TayarDelivery.Areas.Admin.Controllers;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Helper;
using TayarDelivery.Models.ViewModel.Auth;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.Areas.Admin.Controllers.Auth
{
    [Authorize(Roles = "administrator")]
    public class LinkController : BaseController
    {
        protected readonly IBaseRepository<Link> _linkRepository;

        public LinkController(
          IBaseRepository<Link> linkRepository,
          UserManager<ApplicationUser> userManager,
          RoleManager<IdentityRole> roleManager,
          IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._linkRepository = linkRepository;
        }
        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _linkRepository.Filter(
                filter: x => (d.SearchKey == null || x.Title.Contains(d.SearchKey)),
                orderBy: x => x.OrderBy(x => x.CreateAt),
                include: x => x.Include(c => c.Parent)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.Title,
                x.Url,
                x.IconName,
                x.IsActive,
                ParentName = x.Parent != null ? x.Parent.Title : "-",
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
        public IActionResult Index()
        {
            return View();
        }
        // GET: Admin/Link/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ListParentLink"] = new SelectList(await _linkRepository.Find(x => x.ParentId == null), "Id", "Title");
            return View();
        }

      //  POST: Admin/Link/Create
       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LinkVM model )
        {
            if (ModelState.IsValid)
            {
                var newEntity = _mapper.Map<LinkVM, Link>(model);
                
                var result = await _linkRepository.AddAsync(newEntity);
                if (result != null)
                    return Content(ShowMessage.AddSuccessResult(), "application/json");
                return Content(ShowMessage.FailedResult(), "application/json");
            }
            ViewData["ListParentLink"] = new SelectList(await _linkRepository.Find(x => x.ParentId == null), "Id", "Title");
            return View(model);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _linkRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            var newEntity = _mapper.Map<LinkVM>(model);
            ViewData["ListParentLink"] = new SelectList(await _linkRepository.Find(x => x.ParentId == null), "Id", "Title");
            return View(newEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LinkVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _linkRepository.Get(id);
                    PropertyCopy.Copy(model, baseEntoty);
                   
                    await _linkRepository.UpdateAsync(baseEntoty);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await LinkExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Content(ShowMessage.EditSuccessResult(), "application/json");
            }
            ViewData["ListParentLink"] = new SelectList(await _linkRepository.Find(x => x.ParentId == null), "Id", "Title");
            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _linkRepository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            var result = await _linkRepository.DeleteAsync(model);
            if (result != null)
                return Content(ShowMessage.DeleteSuccessResult(), "application/json");
            return Content(ShowMessage.FailedResult(), "application/json");
        }

        private async Task<bool> LinkExists(int id)
        {
            return await _linkRepository.Any(e => e.Id == id);
        }
    }
}

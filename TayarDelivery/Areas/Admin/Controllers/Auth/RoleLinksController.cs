using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Models.ViewModel.Auth;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.Areas.Admin.Controllers.Auth
{
    public class RoleLinksController : BaseController
    {
        private IBaseRepository<RoleLinks> _roleLinksRepository;
        private IBaseRepository<Link> _linkRepository;
        private IBaseRepository<Role> _roleRepository;

        public RoleLinksController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IBaseRepository<RoleLinks> roleLinksRepository,
            IBaseRepository<Link> linkRepository,
            IBaseRepository<Role> roleRepository) : base(userManager, roleManager, mapper)
        {
            this._roleLinksRepository = roleLinksRepository;
            this._linkRepository = linkRepository;
            this._roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<ActionResult> DeleteLinks(int roleid)
        {
            var rolelinks = await _roleLinksRepository.Get().Where(x => x.RoleId == roleid).ToListAsync();
            if (rolelinks.Count() > 0)
                await _roleLinksRepository.DeleteRangeAsync(rolelinks);

            return Json(new
            {
                status = 1,
                msg = "s: تم الحذف بنجاح",
                redirect = "/Admin/RoleLinks/AddLinks/" + roleid
            });
        }

        [NonAction]
        private async Task DeleteAllLinks(int roleid)
        {
            var rolelinks = await _roleLinksRepository.Get().Where(x => x.RoleId == roleid).ToListAsync();
            if (rolelinks.Count() > 0)
                await _roleLinksRepository.DeleteRangeAsync(rolelinks);
        }

        [HttpGet]
        public async Task<ActionResult> AddLinks(int id)
        {
            try
            {
                RoleLinksVM roleLinksVM = await GetRoleLinksVM(id);
                return View(roleLinksVM);
            }
            catch
            {
                ViewData["msg"] = "عذرا لقد حدث خطأ ما!";
                return View(new RoleLinksVM { });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddLinks(int id, int[] links)
        {
            try
            {
                await DeleteAllLinks(id);
                List<RoleLinks> RoleLinksList = new List<RoleLinks>();
                for (int i = 0; i < links.Count(); i++)
                {
                    var RoleLinks = new RoleLinks
                    {
                        LinkId = links[i],
                        RoleId = id
                    };
                    RoleLinksList.Add(RoleLinks);
                }
                await _roleLinksRepository.AddRangeAsync(RoleLinksList);
                RoleLinksVM roleLinksVM = await GetRoleLinksVM(id);
                ViewData["msg"] = "تمت عملية حفظ الصلاحيات بنجاح";
                return View(roleLinksVM);
            }
            catch (Exception e)
            {
                ViewData["msg"] = "عذرا لقد حدث خطأ ما!";
                return View(new RoleLinksVM { });
            }
        }

        private async Task<RoleLinksVM> GetRoleLinksVM(int roleid)
        {
            RoleLinksVM roleLinksVM = new RoleLinksVM();
            List<GroupLink> groupList = new List<GroupLink>();

            var links = await _linkRepository.Get().Select(x => new Link
            {
                Id = x.Id,
                Title = x.Title,
                Url = x.Url,
                Parent = x.Parent,
                ParentId = x.ParentId,
                IsShow = _roleLinksRepository.Get().Any(p => p.LinkId == x.Id && p.RoleId == roleid),
            }).ToListAsync();

            foreach (var item in links.Where(x => x.ParentId == null).ToList())
            {
                GroupLink group = new GroupLink();
                group.parintName = item.Title;
                group.parintId = item.Id;
                group.links = new List<Link>();
                foreach (var i in links)
                {
                    if (i.ParentId != null)
                    {
                        if (i.ParentId == item.Id)
                        {
                            group.links.Add(i);
                        }
                    }
                    if (i.ParentId == null)
                    {
                        if (i.Id == item.Id)
                        {
                            group.links.Add(i);
                        }
                    }
                }
                groupList.Add(group);
            }
            var roleItem = await _roleRepository.Get(roleid);
            roleLinksVM.Role = roleItem;
            roleLinksVM.GroupLinks = groupList;
            return roleLinksVM;
        }
    }
}

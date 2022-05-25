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
using TayarDelivery.Entity.DTO;
using TayarDelivery.Helper;
using TayarDelivery.Models.ViewModel.Mail;
using TayarDelivery.Repository.Interfaces;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.Areas.Admin.Controllers.Notification
{
    public class NotificationController : BaseController
    {
        protected readonly IFCMSender _iFCMSender;
        protected readonly IBaseRepository<Entity.Domins.Notification.Notification> _notificationRepository;
        protected readonly IBaseRepository<UserType> _userTypeRepository;
        public NotificationController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IFCMSender IFCMSender,
            IBaseRepository<Entity.Domins.Notification.Notification> notificationRepository,
            IBaseRepository<UserType> userTypeRepository)
            : base(userManager, roleManager, mapper)
        {
            _iFCMSender = IFCMSender;
            _notificationRepository = notificationRepository;
            _userTypeRepository = userTypeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = _notificationRepository.Filter(
                filter: x => (d.SearchKey == null || x.Title.Contains(d.SearchKey) || x.Message.Contains(d.SearchKey)) &&
                    (x.ReceverId.Equals(UserId) || x.SenderId.Equals(UserId)),
                orderBy: x => x.OrderByDescending(x => x.SendDateAt),
                include: x => x.Include(c => c.ApplicationUserRecever).Include(c => c.ApplicationUserSender)).ToList();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.Title,
                x.Message,
                x.SenderId,
                SenderName = x.ApplicationUserSender.FullName,
                x.ReceverId,
                ReceverName = x.ApplicationUserRecever.FullName,
                Type = x.SenderId.Equals(UserId) ? "FROM" : "TO",
                createAt = x.SendDateAt.Value.ToString("MM/dd/yyyy"),
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

        public IActionResult SendNotfication()
        {
            ViewData["UsersTypeList"] = new SelectList(_userTypeRepository.GetAll().Result.ToList(), "Id", "TitleView");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendNotfication(SendNotficationVM notification)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < notification.ReceversId.Count(); i++)
                {
                    var user = await _userManager.FindByIdAsync(notification.ReceversId[i]);
                    RootNotification root = new RootNotification
                    {
                        to = user.FcmToken,
                        notification = new NotificationDTO
                        {
                            title = notification.Title,
                            body = notification.Messege,
                        },
                        data = new DataNotification
                        {
                            title = notification.Title,
                            body = notification.Messege,
                        }
                    };
                    bool result = await _iFCMSender.Send(root, user.FcmToken);
                    if (result)
                    {
                        await AddMessegeUser(user, notification.Messege, notification.Title);
                    }
                }
                return Content(ShowMessage.SendSuccessResult(), "application/json");
            }
            ViewData["UsersTypeList"] = new SelectList(_userTypeRepository.GetAll().Result.ToList(), "Id", "TitleView");
            return View(notification);
        }

        [HttpGet]
        public async Task<IActionResult> getLastNotification()
        {
            var data = await _notificationRepository.Filter(
                filter: x => x.ReceverId.Equals(UserId),
                take: 15,
                orderBy: x => x.OrderByDescending(c => c.SendDateAt),
                include: x => x.Include(c => c.ApplicationUserRecever)).Select(x => new
                {
                    Title = x.Title,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    ReceverName = x.ApplicationUserRecever.FullName,
                    SendDateAt = x.SendDateAt.Value.ToString("MM/dd/yyyy hh:mm tt")
                }).ToListAsync();
            return Json(data);
        }

        [HttpGet]
        public IActionResult getCountNotificationUnRead()
        {
            try
            {
                var data = _notificationRepository.GetCount(x => x.ReceverId.Equals(UserId) && !x.IsRead);
                return Json(data);
            }
            catch
            {
                return Json(0);
            }
        }

        [HttpGet]
        public async Task setAllNotificationAsRead()
        {
            var notifications = await _notificationRepository.Find(x => x.ReceverId.Equals(UserId));
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _notificationRepository.UpdateRangeAsync(notifications);
        }

        [HttpGet]
        public async Task<IActionResult> SetFCMToken(string token)
        {
            if (!String.IsNullOrEmpty(token))
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    return NotFound();
                }
                user.FcmToken = token;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            return null;
        }

        private async Task<Entity.Domins.Notification.Notification> AddMessegeUser(ApplicationUser user, string message, string title)
        {
            var entityModel = new Entity.Domins.Notification.Notification
            {
                IsRead = false,
                Message = message,
                Title = title,
                ReceverId = user.Id,
                SenderId = UserId
            };
            var result = await _notificationRepository.AddAsync(entityModel);
            return result;
        }
    }
}

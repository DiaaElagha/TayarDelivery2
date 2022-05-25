using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TayarDelivery.API.Dto.Base;
using TayarDelivery.API.Dto.Enums;
using TayarDelivery.API.Helpers;
using TayarDelivery.Data.Data;
using TayarDelivery.Data.StaticModel;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.Notification;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.API.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly IBaseRepository<Notification> _notificationsRepository;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<OrderStatus> _orderStatusRepository;

        public NotificationController(
            IBaseRepository<Notification> notificationsRepository,
            IBaseRepository<Order> orderRepository,
            IBaseRepository<OrderStatus> orderStatusRepository,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper)
            : base(configuration, userManager, mapper)
        {
            _notificationsRepository = notificationsRepository;
            _orderRepository = orderRepository;
            _orderStatusRepository = orderStatusRepository;
        }

        [HttpPost]
        [Authorize(Roles = UserTypeValues.TRADER + "," + UserTypeValues.DRIVER)]
        public async Task<IActionResult> GetNotification(int page)
        {
            if (ModelState.IsValid)
            {
                var itemUser = await GetCurrentUser();

                var notificationsCount = _notificationsRepository.GetCount(x => x.ReceverId.Equals(itemUser.Id));

                var rowPerPage = 10;

                double numberOfPages = Math.Ceiling(notificationsCount / (rowPerPage * 1.0));

                var skipValue = (page - 1) * rowPerPage;

                if (page < 1 || page > numberOfPages + 1)
                {
                    return GetResponse("Page number not valid", false, null);
                }
                var notifications = await _notificationsRepository.Filter(
                    filter: x => x.ReceverId.Equals(itemUser.Id),
                    skip: skipValue,
                    take: rowPerPage,
                    orderBy: x => x.OrderByDescending(v => v.SendDateAt)).ToListAsync();

                PagingResponse pagingResponse = new PagingResponse()
                {
                    TotalPageNumber = (int)numberOfPages,
                    CurrentPage = page,
                    NumberOfRows = notifications.Count(),
                    Data = notifications
                };
                return GetResponse(ResponseMessages.READ, true, pagingResponse);
            }
            return Ok(General.GetValidationErrores(ModelState));
        }

        [HttpPost]
        public async Task<IActionResult> SetNotificationAsRead()
        {
            var itemUser = await GetCurrentUser();

            var notifications = await _notificationsRepository.Find(x => x.ReceverId.Equals(itemUser.Id));

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _notificationsRepository.UpdateRangeAsync(notifications);

            return GetResponse(ResponseMessages.UPDATE, true);
        }

        [HttpGet]
        public async Task<IActionResult> GetNumberNotificationUnRead()
        {
            var itemUser = await GetCurrentUser();

            var notificationsCount = _notificationsRepository.GetCount(x => x.ReceverId.Equals(itemUser.Id) && !x.IsRead);

            return GetResponse(ResponseMessages.READ, true, notificationsCount);
        }

        [HttpPost]
        public async Task<IActionResult> SetDriverApprovalStatus(int notificationId, bool status)
        {
            if (ModelState.IsValid)
            {
                var itemUser = await GetCurrentUser();

                var notificationItem = await _notificationsRepository.Get(notificationId);
                notificationItem.Status = status;
                await _notificationsRepository.UpdateAsync(notificationItem);

                if (notificationItem.OrderId == null)
                    return GetResponse("OrderId is invalid", false);
                var orderItem = await _orderRepository.Get(notificationItem.OrderId);
                if (status)
                {
                    orderItem.UserDriverId = itemUser.Id;
                    orderItem.DriverApprovalStatus = status;
                    orderItem.IsSetDriver = status;
                    orderItem.AllowEdit = false;
                    var sss = await _orderStatusRepository.FindFirst(x => x.TitlePrograming.Equals(OrderStatusValues.BEINGDELIVERED));
                    orderItem.OrderStatusId = sss.Id;
                }
                else
                {
                    orderItem.UserDriverId = null;
                    orderItem.DriverApprovalStatus = !status;
                    orderItem.IsSetDriver = !status;
                }
                await _orderRepository.UpdateAsync(orderItem);
                return GetResponse(ResponseMessages.UPDATE, true);
            }
            return Ok(General.GetValidationErrores(ModelState));
        }


    }
}

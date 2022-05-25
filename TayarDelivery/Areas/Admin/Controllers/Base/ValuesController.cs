using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Data.StaticModel;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.Areas.Admin.Controllers.Base
{
    public class ValuesController : BaseController
    {
        protected IBaseRepository<OrderStatus> _orderStatusRepository;
        protected IBaseRepository<Order> _orderRepository;

        public ValuesController(
            IBaseRepository<OrderStatus> orderStatusRepository,
            IBaseRepository<Order> orderRepository,

            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(userManager, roleManager, mapper)
        {
            this._orderStatusRepository = orderStatusRepository;
            this._orderRepository = orderRepository;
        }

        public async Task<IActionResult> GetTradersFullData()
        {
            var listUsers = await _userManager.Users.Include(x => x.UserType).Include(x => x.PriceType).Include(x => x.Area)
                .Where(x => x.IsActive && x.UserType.TitlePrograming.Equals(UserTypeValues.TRADER))
                .Select(x => new
                {
                    AreaId = x.Area != null ? x.Area.Id : 0,
                    AreaName = x.Area != null ? x.Area.Name : "",
                    PriceTypeValue = x.PriceType != null ? x.PriceType.DiscountPercentage : 0,
                    TraderId = x.Id,
                    TraderName = x.FullName,
                }).ToListAsync();
            return Ok(listUsers);
        }

        public async Task<IActionResult> GetOrderStatus()
        {
            var listData = await _orderStatusRepository.Get().Select(x => new
            {
                Id = x.Id,
                TitleView = x.TitleView,
                TitlePrograming = x.TitlePrograming,
                CountOrders = _orderRepository.Get().Count(c => c.OrderStatusId == x.Id)
            }).ToListAsync();
            return Json(listData);
        }


    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TayarDelivery.Data.Data;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Helper;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.Repository.Repository.Repositores
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _DbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<float> CalculateOrderPrice(
            float mainPrice, float? additionalCost, float? discountedCost,
            string traderId, int areaIdReceiver, int areaIdSender, int orderTypeId, bool isIncludeDeliveryPrice)
        {
            var traderEntity = await _userManager.Users.Include(x => x.PriceType).SingleOrDefaultAsync(x => x.Id.Equals(traderId));
            if (traderEntity != null)
            {
                float? priceDelvery = 0;
                float? orderTypeDiscount = 0;

                var areaPrice = await _DbContext.AreasPrice.FirstOrDefaultAsync(x =>
                    (x.ReceverAreaId == areaIdReceiver && x.DealerAreaId == areaIdSender)
                    || (x.ReceverAreaId == areaIdSender && x.DealerAreaId == areaIdReceiver));

                var orderType = await _DbContext.OrderType.FindAsync(orderTypeId);
                
                if (orderType != null && areaPrice != null)
                {
                    if (!isIncludeDeliveryPrice)
                    {
                        priceDelvery = areaPrice.Price;
                        orderTypeDiscount = orderType.DiscountPercentage;
                    }
                    else
                    {
                        mainPrice = (mainPrice - (areaPrice.Price.HasValue ? areaPrice.Price.Value : 0)) - orderType.DiscountPercentage.Value;
                        orderTypeDiscount = 0;
                    }
                    if (additionalCost.HasValue)
                    {
                        priceDelvery += additionalCost.Value;
                    }
                    if (discountedCost.HasValue)
                    {
                        priceDelvery -= discountedCost.Value;
                    }
                    float? totalPrice = ExtensionMethods.GetTotalPriceOrder(
                               price: priceDelvery,
                               mainPrice: mainPrice,
                               discount: traderEntity.PriceType.DiscountPercentage,
                               plus: orderTypeDiscount);
                    return totalPrice.HasValue ? totalPrice.Value : 0;
                }
            }
            return -1;
        }


    }
}

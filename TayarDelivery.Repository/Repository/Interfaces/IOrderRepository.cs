using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TayarDelivery.Repository.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<float> CalculateOrderPrice(
            float mainPrice, float? additionalCost, float? discountedCost,
            string traderId, int areaIdReceiver, int areaIdSender, int orderTypeId, bool isIncludeDeliveryPrice);
    }
}

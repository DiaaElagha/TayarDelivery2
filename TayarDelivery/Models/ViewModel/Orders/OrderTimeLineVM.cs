using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Models.ViewModel.Orders
{
    public class OrderTimeLineVM
    {
        public List<OrderHistory> orderHistories { get; set; } = new List<OrderHistory>();
        public Order Order { get; set; } = new Order();
    }
}

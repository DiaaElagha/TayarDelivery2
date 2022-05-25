using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Models.ViewModel.Orders
{
    public class BillErsaliaVM
    {
        public BillErsaliaVM()
        {
            listOrders = new List<Order>();
        }
        public List<Order> listOrders { set; get; }
    }
}

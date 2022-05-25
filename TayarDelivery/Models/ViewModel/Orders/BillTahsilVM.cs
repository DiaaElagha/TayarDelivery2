using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Models.ViewModel.Orders
{
    public class BillTahsilVM
    {
        public List<Order> listOrders { set; get; } = new List<Order>();
        public ApplicationUser traderItem { get; set; }
        public string noteTahsil { get; set; }
    }
}

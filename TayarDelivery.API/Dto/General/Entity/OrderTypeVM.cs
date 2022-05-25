using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.General.Entity
{
    public class OrderTypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float? DiscountPercentage { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}

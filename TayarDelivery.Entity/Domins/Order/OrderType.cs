using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins
{
    public class OrderType : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float? DiscountPercentage { get; set; }
    }
}

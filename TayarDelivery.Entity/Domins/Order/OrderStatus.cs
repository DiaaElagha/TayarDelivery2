using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins
{
    public class OrderStatus :BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string TitlePrograming { get; set; }
        public string TitleView { get; set; }
        public string Color { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.General.Entity
{
    public class OrderStatusVM
    {
        public int Id { get; set; }
        public string TitlePrograming { get; set; }
        public string TitleView { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}

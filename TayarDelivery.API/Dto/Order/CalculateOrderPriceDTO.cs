using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.Order
{
    public class CalculateOrderPriceDTO
    {
        [Required]
        public float mainPrice { get; set; }
        [Required]
        public float additionalCost { get; set; }
        [Required]
        public float discountedCost { get; set; }
        [Required]
        public int areaIdReceiver { get; set; }
        [Required]
        public int areaIdSender { get; set; }
        [Required]
        public int orderTypeId { get; set; }
        [Required]
        public bool isIncludeDeliveryPrice { get; set; }
    }
}

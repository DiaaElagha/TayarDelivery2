using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.Order
{
    public class OrderAddDTO
    {
        public string Title { get; set; }
        [Required]
        public string NameReceiver { get; set; }
        [Required]
        public string PhoneNumberReceiver { get; set; }
        public string PhoneNumberReceiver2 { get; set; }
        [Required]
        public string Address { get; set; }
        public string Description { get; set; }
        [Required]
        public float MainPrice { get; set; }
        [Required]
        public float? TotalPrice { get; set; }
        public string NoteTrader { get; set; }
        public string NoteDriver { get; set; }
        public double? CustomerLongitude { get; set; }
        public double? CustomerLatitude { get; set; }
        [Required]
        public int OrderTypeId { get; set; }
        [Required]
        public int AreaIdReceiver { get; set; }
        [Required]
        public int AreaIdSender { get; set; }
        [Required]
        public int OrderContentId { get; set; }

        [Required]
        public bool IsIncludeDeliveryPrice { get; set; }

    }
}

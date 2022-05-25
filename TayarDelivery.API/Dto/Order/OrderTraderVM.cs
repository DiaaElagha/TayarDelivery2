using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.API.Dto.General.Entity;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.API.Dto.Order
{
    public class OrderTraderVM
    {
        public int OrderId { get; set; }
        public string Title { get; set; }

        public string SerialNumber { get; set; }
        public string NameReceiver { get; set; }
        public string PhoneNumberReceiver { get; set; }
        public string PhoneNumberReceiver2 { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public float? MainPrice { get; set; }
        public float? TotalPrice { get; set; }

        public bool? IsIncludeDeliveryPrice { get; set; }
        public bool? AllowEdit { get; set; }
        public string NoteTrader { get; set; }
        public string NoteDriver { get; set; }

        public string FilePathTraderSignature { get; set; }

        public double? CustomerLongitude { get; set; }
        public double? CustomerLatitude { get; set; }

        public int? AreaIdReceiver { get; set; }
        [ForeignKey(nameof(AreaIdReceiver))]
        public AreaVM Area { get; set; }

        public string UserDriverId { get; set; }
        [ForeignKey(nameof(UserDriverId))]
        public ApplicationUser ApplicationUserDriver { get; set; }

        public int? OrderStatusId { get; set; }
        [ForeignKey(nameof(OrderStatusId))]
        public OrderStatusVM OrderStatus { get; set; }

        public int? OrderTypeId { get; set; }
        [ForeignKey(nameof(OrderTypeId))]
        public OrderTypeVM OrderType { get; set; }

        public int? OrderContentId { get; set; }
        [ForeignKey(nameof(OrderContentId))]
        public OrderContent OrderContent { get; set; }

        public DateTime? CreateAt { get; set; }
    }
}

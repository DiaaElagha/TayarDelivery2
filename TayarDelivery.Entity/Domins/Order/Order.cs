using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TayarDelivery.Entity.Domins.Base;
using TayarDelivery.Entity.Domins.LookUp;

namespace TayarDelivery.Entity.Domins
{
    public class Order : BaseEntity
    {
        [Key]
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
        public float? AdditionalCost { get; set; }
        public float? DiscountedCost { get; set; }


        [ScaffoldColumn(false)]
        public bool AllowEdit { get; set; }
        [ScaffoldColumn(false)]
        public bool IsDeliveredSuccess { get; set; }
        public bool? IsIncludeDeliveryPrice { get; set; }

        [ScaffoldColumn(false)]
        public bool DriverApprovalStatus { get; set; }

        [ScaffoldColumn(false)]
        public bool IsSetDriver { get; set; }

        [ScaffoldColumn(false)]
        public bool IsArchive { get; set; }

        public string NoteTrader { get; set; }
        public string NoteDriver { get; set; }

        public string FilePathTraderSignature { get; set; }

        public double? CustomerLongitude { get; set; }
        public double? CustomerLatitude { get; set; }

        public int? AreaIdSender { get; set; }
        [ForeignKey(nameof(AreaIdSender))]
        public Area AreaSender { get; set; }

        public int? AreaIdReceiver { get; set; }
        [ForeignKey(nameof(AreaIdReceiver))]
        public Area Area { get; set; }

        public string UserTraderId { get; set; }
        [ForeignKey(nameof(UserTraderId))]
        public ApplicationUser ApplicationUserTrader { get; set; }

        public string UserDriverId { get; set; }
        [ForeignKey(nameof(UserDriverId))]
        public ApplicationUser ApplicationUserDriver { get; set; }

        public int? OrderStatusId { get; set; }
        [ForeignKey(nameof(OrderStatusId))]
        public OrderStatus OrderStatus { get; set; }

        public int? OrderTypeId { get; set; }
        [ForeignKey(nameof(OrderTypeId))]
        public OrderType OrderType { get; set; }

        public int? OrderContentId { get; set; }
        [ForeignKey(nameof(OrderContentId))]
        public OrderContent OrderContent { get; set; }

        
    }
}

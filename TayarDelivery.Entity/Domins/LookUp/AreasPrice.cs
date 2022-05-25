using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins.LookUp
{
    public class AreasPrice : BaseEntity
    {
        public int ReceverAreaId { get; set; }
        [ForeignKey(nameof(ReceverAreaId))]
        public Area ReceverArea { get; set; }

        public int DealerAreaId { get; set; }
        [ForeignKey(nameof(DealerAreaId))]
        public Area DealerArea { get; set; }

        public float? Price { get; set; }

        public float? DiscountPriceWhenReturn { get; set; }

        [ScaffoldColumn(false)]
        public bool CanDiscount { get; set; }

    }
}

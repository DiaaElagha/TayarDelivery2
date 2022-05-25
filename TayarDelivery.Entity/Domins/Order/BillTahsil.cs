using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins
{
    public class BillTahsil : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string TraderId { get; set; }
        [ForeignKey(nameof(TraderId))]
        public ApplicationUser ApplicationUserTrader { get; set; }

        public string FilePath { get; set; }
        public float? TotalPrice { get; set; }
        public int? NumberOfOrder { get; set; }
    }
}

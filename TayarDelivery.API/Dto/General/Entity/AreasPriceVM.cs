using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.General.Entity
{
    public class AreasPriceVM
    {
        public int ReceverAreaId { get; set; }
        [ForeignKey(nameof(ReceverAreaId))]
        public AreaVM ReceverArea { get; set; }

        public int DealerAreaId { get; set; }
        [ForeignKey(nameof(DealerAreaId))]
        public AreaVM DealerArea { get; set; }

        public float? Price { get; set; }

        public bool CanDiscount { get; set; }

        public DateTime? CreateAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TayarDelivery.Entity.Domins
{
    public partial class RoleLinks
    {
        public int LinkId { get; set; }
        [ForeignKey(nameof(LinkId))]
        public virtual Link Link { get; set; }

        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
    }
}

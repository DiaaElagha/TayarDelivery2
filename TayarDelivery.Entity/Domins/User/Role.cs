using System;
using System.Collections.Generic;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins
{
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins
{
    public class Link : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Decription { get; set; }
        public string Url { get; set; }
        public bool IsShow { get; set; }
        public bool IsActive { get; set; }
        public string IconName { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Link Parent { get; set; }
       
    }
}

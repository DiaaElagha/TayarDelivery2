using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TayarDelivery.Entity.Domins.Base
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreateAt = DateTime.Now;
        }
        [ScaffoldColumn(false)]
        public DateTime? CreateAt { get; set; }

        public string CreateByUserId { get; set; }
        [ForeignKey(nameof(CreateByUserId))]
        public ApplicationUser ApplicationUserCreate { get; set; }

        public string UpdateByUserId { get; set; }
        [ForeignKey(nameof(UpdateByUserId))]
        public ApplicationUser ApplicationUserUpdate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdateAt { get; set; }
    }
}

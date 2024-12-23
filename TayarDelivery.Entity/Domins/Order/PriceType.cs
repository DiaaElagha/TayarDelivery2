﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins
{
    public class PriceType : BaseEntity
    {
        public PriceType()
        {
            ApplicationUser = new HashSet<ApplicationUser>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float? DiscountPercentage { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}

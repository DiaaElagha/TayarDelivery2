using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins.LookUp
{
    public class Area : BaseEntity
    {
        public Area()
        {
            ApplicationUser = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }


        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}

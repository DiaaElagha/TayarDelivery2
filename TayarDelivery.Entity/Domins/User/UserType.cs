using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins
{
    public class UserType : BaseEntity
    {
        public UserType()
        {
            ApplicationUser = new HashSet<ApplicationUser>();
        }
        [Key]
        public int Id { get; set; }
        public string TitlePrograming { get; set; }
        public string TitleView { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}

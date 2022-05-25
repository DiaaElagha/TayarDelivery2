using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TayarDelivery.Entity.Domins.User
{
    public class UserProfile 
    {
        public UserProfile()
        {
            AllowNotification = true;
            AllowEdit = true;
            AllowAdd = true;
            AllowRemove = true;
        }
        [Key]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUserDriver { get; set; }

        public float? RatingCompany { get; set; }
        public bool AllowNotification { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowRemove { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Models.ViewModel.Auth
{
    public class UserRolesVM
    {
        public ApplicationUser User { get; set; }
        public SelectList RoleList { get; set; }

        [Display(Name = "الصلاحيات")]
        public int[] RolesId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.Models.ViewModel.Auth
{
    public class LinkVM
    {
        [Required]
        [Display(Name = "العنوان")]
        public string Title { get; set; }
        [Display(Name = "الوصف")]
        public string Decription { get; set; }
        [Display(Name = "الرابط")]
        public string Url { get; set; }
        [Display(Name = "العرض")]
        public bool IsShow { get; set; }
        [Display(Name = "الفعالية")]
        public bool IsActive { get; set; }
        [Display(Name = "الأيقونة")]
        public string IconName { get; set; }
        [Display(Name = "الرابط الأساسي")]
        public int? ParentId { get; set; }
    }
}

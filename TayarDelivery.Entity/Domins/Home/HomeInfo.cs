using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TayarDelivery.Entity.Domins.Base;

namespace TayarDelivery.Entity.Domins.Home
{
    public class HomeInfo
    {
        [Key]
        public int Id { get; set; }
        public string MainTitle { get; set; }
        public string MainTitleColor { get; set; }
        public string SubTitle { get; set; }
        public string SubTitleColor { get; set; }
        public string BackgroundImage { get; set; }
        public string BackgroundImageWidth { get; set; }
        public string BackgroundImageHeight { get; set; }

        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.General.Auth
{
    public class UserProfileSettingsDto
    {

        public float? RatingCompany { get; set; }
        public bool AllowNotification { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowRemove { get; set; }
    }
}

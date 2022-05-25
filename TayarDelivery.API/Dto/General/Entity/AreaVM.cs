using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.General.Entity
{
    public class AreaVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}

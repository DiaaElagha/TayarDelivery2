using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.Base
{
    public class APIResponse
    {
        public Boolean Status { get; set; }
        public Object Message { get; set; }
        public Object Data { get; set; }
    }
}

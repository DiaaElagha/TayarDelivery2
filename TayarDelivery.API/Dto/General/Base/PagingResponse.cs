using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.Base
{
    public class PagingResponse
    {
        public int TotalPageNumber { get; set; }
        public int CurrentPage { get; set; }
        public int NumberOfRows { get; set; }
        public object Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TayarDelivery.API.Dto.Enums
{
    public enum ResponseMessages
    {
        [Description("Added successfully")]
        CREATE,
        [Description("Updated successfully")]
        UPDATE,
        [Description("Get Data successfully")]
        READ,
        [Description("Deleted successfully")]
        DELETE,
        [Description("Something went wrong")]
        FAILED,
    }
}

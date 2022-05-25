using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Models.ViewModel.Auth
{
    public class RoleLinksVM
    {
        public Role Role { get; set; }
        public List<GroupLink> GroupLinks { get; set; }

    }

    public class GroupLink
    {
        public int parintId { get; set; }
        public string parintName { get; set; }
        public List<Link> links { get; set; }
    }
}

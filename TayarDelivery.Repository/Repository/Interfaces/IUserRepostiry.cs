using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Repository.Repository.Interfaces
{
    public interface IUserRepostiry
    {
        /// <summary>
        /// Get all entities on DB with filter
        /// </summary>
        /// <returns>List of entities by filter</returns>
        IQueryable<ApplicationUser> FilterAllUsers(Expression<Func<ApplicationUser, bool>> filter,
                                              int skip = 0,
                                              int take = int.MaxValue,
                                              Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null,
                                              Func<IQueryable<ApplicationUser>, IIncludableQueryable<ApplicationUser, object>> include = null);
    }
}

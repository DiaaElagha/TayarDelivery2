using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TayarDelivery.Data.Data;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Repository.Repository.Interfaces;

namespace TayarDelivery.Repository.Repository.Repositores
{
    public class UserRepostiry : IUserRepostiry
    {
        private readonly ApplicationDbContext _DbContext;

        public UserRepostiry(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public IQueryable<ApplicationUser> FilterAllUsers(
                Expression<Func<ApplicationUser, bool>> filter,
                int skip = 0, int take = int.MaxValue,
                Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null,
                Func<IQueryable<ApplicationUser>, IIncludableQueryable<ApplicationUser, object>> include = null)
        {
            var _resetSet = filter != null ? _DbContext.Users.AsNoTracking().Where(filter).AsQueryable()
               : _DbContext.Users.AsNoTracking().AsQueryable();

            if (include != null)
            {
                _resetSet = include(_resetSet);
            }
            if (orderBy != null)
            {
                _resetSet = orderBy(_resetSet).AsQueryable();
            }
            _resetSet = skip == 0 ? _resetSet.Take(take) : _resetSet.Skip(skip).Take(take);

            return _resetSet.AsQueryable();
        }
    }
}

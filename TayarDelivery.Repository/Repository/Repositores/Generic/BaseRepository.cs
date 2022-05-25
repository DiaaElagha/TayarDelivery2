using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TayarDelivery.Data.Data;
using TayarDelivery.Entity.DTO;
using TayarDelivery.Repository.Interfaces;

namespace TayarDelivery.Repository.Repositores
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new() 
    {
        private readonly ApplicationDbContext _DbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            try
            {
                return await _DbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter,
                                              int skip = 0,
                                              int take = int.MaxValue,
                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var _resetSet = filter != null ? _DbContext.Set<TEntity>().AsNoTracking().Where(filter).AsQueryable() 
                : _DbContext.Set<TEntity>().AsNoTracking().AsQueryable();

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

        public virtual IQueryable<TEntity> Get()
        {
            try
            {
                return _DbContext.Set<TEntity>().AsNoTracking();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        
        public virtual async Task<TEntity> Get<T>(T key)
        {
            try
            {
                return await _DbContext.Set<TEntity>().FindAsync(key);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return _DbContext.Set<TEntity>().Count(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }

            try
            {
                await _DbContext.AddAsync(entity);
                await _DbContext.SaveChangesAsync(); 
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException($"{nameof(entities)} entity must not be null");
            }

            try
            {
                await _DbContext.AddRangeAsync(entities);
                await _DbContext.SaveChangesAsync();
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be saved: {ex.Message}");
            }
        }
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }

            try
            {
                _DbContext.Update(entity);
                await _DbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public virtual async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException($"{nameof(entities)} entity must not be null");
            }
            try
            {
                _DbContext.UpdateRange(entities);
                await _DbContext.SaveChangesAsync();

                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be updated: {ex.Message}");
            }
        }

        public virtual async Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await _DbContext.Set<TEntity>().Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> FindSingle(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await _DbContext.Set<TEntity>().SingleOrDefaultAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entity: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> FindFirst(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await _DbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entity: {ex.Message}");
            }
        }

        public virtual async Task<PagingDTO> FindPaging(Expression<Func<TEntity, bool>> expression, int skip, int take)
        {
            try
            {
                var entities = _DbContext.Set<TEntity>().Where(expression);
                var count = await entities.CountAsync();
                var dataList = entities.Skip(skip).Take(take).ToListAsync();
                return new PagingDTO()
                {
                    Data = dataList,
                    Total = count
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }
            try
            {
                _DbContext.Remove(entity);
                await _DbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException($"{nameof(entities)} entity must not be null");
            }

            try
            {
                _DbContext.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be updated: {ex.Message}");
            }
        }

        public virtual async Task<bool> Any(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await _DbContext.Set<TEntity>().AnyAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        public void Dispose()
        {
            _DbContext.Dispose();
        }

    }
}
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TayarDelivery.Entity.DTO;

namespace TayarDelivery.Repository.Interfaces
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class, new()
    {
        /// <summary>
        /// Get all entities on DB
        /// </summary>
        /// <returns>List of entities</returns>
        Task<List<TEntity>> GetAll();

        /// <summary>
        /// Get all entities on DB with filter
        /// </summary>
        /// <returns>List of entities by filter</returns>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter,
                                              int skip = 0,
                                              int take = int.MaxValue,
                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        
        /// <summary>
        /// Get single item by Id(key)
        /// </summary>
        /// <typeparam name="T">Type of the key (int, string,..etc)</typeparam>
        /// <param name="key">value of key</param>
        /// <returns></returns>
        Task<TEntity> Get<T>(T key);

        /// <summary>
        /// Get Count for all entities on DB
        /// </summary>
        /// <returns>Count of entities</returns>
        int GetCount(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Find entities with specific conditions
        /// </summary>
        /// <param name="expression">condition to be find with</param>
        /// <returns>List of result entities</returns>
        Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Find single entity with specific conditions
        /// </summary>
        /// <param name="expression">condition to be find with</param>
        /// <returns>Result entity match the condition</returns>
        Task<TEntity> FindSingle(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Find First entity with specific conditions
        /// </summary>
        /// <param name="expression">condition to be find with</param>
        /// <returns>Result entity match the condition</returns>
        Task<TEntity> FindFirst(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Find entities with specific conditions
        /// </summary>
        /// <param name="expression">condition to be find with</param>
        /// <param name="skip">number of skip items</param>
        /// <param name="take">number of itemes to be taken</param>
        /// <returns>List of result entities paging</returns>
        Task<PagingDTO> FindPaging(Expression<Func<TEntity, bool>> expression, int skip, int take);

        /// <summary>
        /// Get entity to apply process on it before send it to DB.
        /// </summary>
        /// <returns>Entity queryable</returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// Add new item to DB
        /// </summary>
        /// <param name="entity">entity object to be added</param>
        /// <returns>the entity after added (with Id)</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Add new items to DB
        /// </summary>
        /// <param name="entities">entity objects to be added </param>
        /// <returns>entities after added (with Id)</returns>
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update exist item on DB
        /// </summary>
        /// <param name="entity">entity object to be updated</param>
        /// <returns>entity after updated</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Update exist items on DB
        /// </summary>
        /// <param name="entities">entity objects to be updated</param>
        /// <returns>entities after updated</returns>
        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Check if there is any entity db that match the expression.
        /// </summary>
        /// <param name="expression">condition to be checked with</param>
        /// <returns>True if there is at least one entity on db match expression</returns>
        Task<bool> Any(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Make soft delete to the entity
        /// </summary>
        /// <param name="entity">entity inherit from BaseEntity </param>
        /// <returns>entity after deleted</returns>
        Task<TEntity> DeleteAsync(TEntity entity);


        /// <summary>
        /// Make soft delete to the entities
        /// </summary>
        /// <param name="entities">entities inherit from BaseEntity </param>
        /// <returns></returns>
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);


    }
}

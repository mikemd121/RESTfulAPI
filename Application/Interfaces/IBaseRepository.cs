using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
/// <summary>
        /// An interface for generic data repository operations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IBaseRepository<T>
        {
            /// <summary>
            /// Returns the rows for type T by filter ,orderBy = null,includeProperties
            /// </summary>
            /// <returns></returns>
            IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int start = 0, int length = 0);

            /// <summary>
            /// Returns the count by filter ,orderBy = null,includeProperties
            /// </summary>
            /// <param name="filter"></param>
            /// <param name="orderBy"></param>
            /// <param name="includeProperties"></param>
            /// <returns></returns>
            int Count(Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                string includeProperties = "");

            /// <summary>
            /// Returns all the rows for type T
            /// </summary>
            /// <returns></returns>
            IEnumerable<T> GetAll();

            /// <summary>
            /// Does this item exist by it's primary key
            /// </summary>
            /// <param name="primaryKey"></param>
            /// <returns></returns>
            bool Exists(object primaryKey);

            /// <summary>
            /// Insert
            /// </summary>
            /// <param name="entity"></param>
            void Insert(T entity);

            /// <summary>
            /// Update
            /// </summary>
            /// <param name="entity"></param>
            void Update(T entity);

            /// <summary>
            /// Deletes this entry from the database by entity
            /// </summary>
            /// <param name="entity">The entity to delete</param>
            /// <returns></returns>
            void Delete(T entity);

            /// <summary>
            /// Deletes this entry from the database by entity
            /// </summary>
            /// <param name="primaryKey"></param>
            void Delete(object primaryKey);

            ///// <summary>
            ///// Deletes this entry from the database by entity
            ///// </summary>
            ///// <param name="filter">filter</param>
            //void Delete(Expression<Func<T, bool>> filter);

            /// <summary>
            /// Find this entry from the database by entity
            /// </summary>
            /// <param name="primaryKey"></param>
            /// <returns></returns>
            T GetById(object primaryKey);
        }
    }


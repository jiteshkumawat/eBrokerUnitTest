using System;
using System.Collections.Generic;
using System.Text;

namespace eBroker.DAL
{
    /// <summary>
    /// Contract for Repository
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    public interface IRepository<T>: IDisposable
    {
        /// <summary>
        /// Flag to identify if object is disposed.
        /// </summary>
        bool DisposedValue { get; set; }

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns>Entity Details</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get Entity By Identifier
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <returns>Entity Details</returns>
        T Get(int id);

        /// <summary>
        /// Insert new Entity
        /// </summary>
        /// <param name="entity">Entity Details</param>
        /// <returns>Inserted Entity Id</returns>
        int Insert(T entity);

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity">Entity Details</param>
        void Update(T entity);

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="id">Entity Id</param>
        void Delete(int id);
    }
}

using System.Collections.Generic;

namespace eBroker.Business
{
    /// <summary>
    /// Contract for Manager
    /// </summary>
    /// <typeparam name="T">Model Type</typeparam>
    public interface IManager<T>
    {
        /// <summary>
        /// Get Model By Identifier
        /// </summary>
        /// <param name="id">Model Identifier</param>
        /// <returns>Model Details</returns>
        T GetById(int id);

        /// <summary>
        /// Get All Models
        /// </summary>
        /// <returns>Model Details</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Insert Model
        /// </summary>
        /// <param name="model">Model Details</param>
        /// <returns>Inserted Model Identifier</returns>
        int Insert(T model);

        /// <summary>
        /// Update Model
        /// </summary>
        /// <param name="model">Model Details</param>
        void Update(T model);

        /// <summary>
        /// Delete Model
        /// </summary>
        /// <param name="id">Model Identifier</param>
        void Delete(int id);
    }
}

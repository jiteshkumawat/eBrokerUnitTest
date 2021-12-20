using eBroker.DAL;
using System;
using System.Collections.Generic;

namespace eBroker.Business
{
    /// <summary>
    /// Manager Base Class
    /// </summary>
    /// <typeparam name="TModel">Model Type</typeparam>
    public class ManagerBase<TModel> : IManager<TModel>
    {
        /// <summary>
        /// Repository Instance
        /// </summary>
        protected IRepository<TModel> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerBase{TModel}"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public ManagerBase(IRepository<TModel> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Delete Model
        /// </summary>
        /// <param name="id">Model Identifier</param>
        public virtual void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.repository.Delete(id);
        }

        /// <summary>
        /// Get All Models
        /// </summary>
        /// <returns>Model Details</returns>
        public virtual IEnumerable<TModel> GetAll()
        {
            return this.repository.GetAll();
        }

        /// <summary>
        /// Get Model By Identifier
        /// </summary>
        /// <param name="id">Model Identifier</param>
        /// <returns>Model Details</returns>
        public virtual TModel GetById(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            return this.repository.Get(id);
        }

        /// <summary>
        /// Insert Model
        /// </summary>
        /// <param name="model">Model Details</param>
        /// <returns>Inserted Model Identifier</returns>
        public virtual int Insert(TModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException();
            }

            return this.repository.Insert(model);
        }

        /// <summary>
        /// Update Model
        /// </summary>
        /// <param name="model">Model Details</param>
        public virtual void Update(TModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException();
            }

            this.repository.Update(model);
        }
    }
}

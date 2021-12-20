using System;
using System.Collections.Generic;
using System.Linq;
using eBroker.Model;
using Microsoft.EntityFrameworkCore;

namespace eBroker.DAL
{
    /// <summary>
    /// Equity Repository
    /// </summary>
    public class EquityRepository : IRepository<Equity>
    {
        /// <summary>
        /// Flag to identify if object is disposed.
        /// </summary>
        public bool DisposedValue { get; set; }

        /// <summary>
        /// Trade Context
        /// </summary>
        private readonly IBrokerContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquityRepository"/> class.
        /// </summary>
        /// <param name="context">Trade Context</param>
        public EquityRepository(IBrokerContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        public void Delete(int id)
        {
            var entity = this.context.Equities.Find(id);
            this.context.Remove(entity);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get Entity By Id
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <returns>Entity Details</returns>
        public Equity Get(int id)
        {
            return this.context.Equities.Find(id);
        }

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns>Entity Details</returns>
        public IEnumerable<Equity> GetAll()
        {
            return this.context.Equities.AsEnumerable();
        }

        /// <summary>
        /// Insert New Entity
        /// </summary>
        /// <param name="entity">Entity Details</param>
        /// <returns>Inserted Identifier</returns>
        public int Insert(Equity entity)
        {
            this.context.Equities.Add(entity);
            this.context.SaveChanges();
            return entity.ID;
        }

        /// <summary>
        /// Update all entities
        /// </summary>
        /// <param name="entity">Entity Details</param>
        public void Update(Equity entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Dispose Repository
        /// </summary>
        /// <param name="disposing">Disposing Flag</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }

                DisposedValue = true;
            }
        }

        /// <summary>
        /// Dispose Repository
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

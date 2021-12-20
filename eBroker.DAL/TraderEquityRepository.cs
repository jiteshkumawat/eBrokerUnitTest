using System;
using System.Collections.Generic;
using System.Linq;
using eBroker.Model;
using Microsoft.EntityFrameworkCore;

namespace eBroker.DAL
{
    /// <summary>
    /// Trader Equity Repository
    /// </summary>
    public class TraderEquityRepository : IRepository<TraderEquity>
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
        /// Initializes a new instance of the <see cref="TraderEquityRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public TraderEquityRepository(IBrokerContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        public void Delete(int id)
        {
            var entity = this.context.TraderEquities.Find(id);
            this.context.Remove(entity);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get Entity By Identifier
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <returns>Entity Details</returns>
        public TraderEquity Get(int id)
        {
            var traderEquities = this.context.TraderEquities
                .Include(t => t.Equity)
                .FirstOrDefault(t => t.ID == id);
            return traderEquities;
        }

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns>Entity Details</returns>
        public IEnumerable<TraderEquity> GetAll()
        {
            var traderEquities = this.context.TraderEquities
                .Include(t => t.Equity)
                .ToList();
            return traderEquities;
        }

        /// <summary>
        /// Insert new Entity
        /// </summary>
        /// <param name="entity">Entity Detail</param>
        /// <returns>Inserted New Entity</returns>
        public int Insert(TraderEquity entity)
        {
            this.context.TraderEquities.Add(entity);
            this.context.SaveChanges();
            return entity.ID;
        }

        /// <summary>
        /// Update new Entity
        /// </summary>
        /// <param name="entity">Entity Details</param>
        public void Update(TraderEquity entity)
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

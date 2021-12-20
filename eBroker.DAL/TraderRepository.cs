using System;
using System.Collections.Generic;
using System.Linq;
using eBroker.Model;
using Microsoft.EntityFrameworkCore;

namespace eBroker.DAL
{
    /// <summary>
    /// Trade Repsotiory
    /// </summary>
    public class TraderRepository : IRepository<Trader>
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
        /// Initializes a new instance of the <see cref="TraderRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public TraderRepository(IBrokerContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <param name="id">Entity Id</param>
        public void Delete(int id)
        {
            var entity = this.context.Traders.Find(id);
            this.context.Remove(entity);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get Entity by Id
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <returns>Entity Details</returns>
        public Trader Get(int id)
        {
            var trader = this.context.Traders
                            .Include(t => t.TraderEquities)
                            .FirstOrDefault(t => t.ID == id);

            if (trader != null && trader.TraderEquities != null)
            {
                foreach (var traderEquity in trader.TraderEquities)
                {
                    traderEquity.Equity = this.context.Equities.Find(traderEquity.EquityId);
                }
            }

            return trader;
        }

        /// <summary>
        /// Get all Entities
        /// </summary>
        /// <returns>Entity Details</returns>
        public IEnumerable<Trader> GetAll()
        {
            var traders = this.context.Traders
                            .Include(t => t.TraderEquities)
                            .ToList();

            foreach (var trader in traders)
            {
                if (trader.TraderEquities != null)
                {
                    foreach (var traderEquity in trader.TraderEquities)
                    {
                        traderEquity.Equity = this.context.Equities.Find(traderEquity.EquityId);
                    }
                }
            }
            return traders;
        }

        /// <summary>
        /// Insert new Entity
        /// </summary>
        /// <param name="entity">Entity details</param>
        /// <returns>Inserted Entity Identifier</returns>
        public int Insert(Trader entity)
        {
            this.context.Traders.Add(entity);
            this.context.SaveChanges();
            return entity.ID;
        }

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity">Entity details</param>
        public void Update(Trader entity)
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

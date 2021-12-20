using System;
using eBroker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eBroker.DAL
{
    /// <summary>
    /// Broker Context Contract
    /// </summary>
    public interface IBrokerContext: IDisposable
    {
        /// <summary>
        /// Traders Datasets
        /// </summary>
        DbSet<Trader> Traders { get; set; }

        /// <summary>
        /// Equities Datasets
        /// </summary>
        DbSet<Equity> Equities { get; set; }

        /// <summary>
        /// TraderEquities Datasets
        /// </summary>
        DbSet<TraderEquity> TraderEquities { get; set; }

        /// <summary>
        /// Save all entity Changes
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Remove Entity
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="entity">Entity to remove</param>
        /// <returns>Removed Entity Entry</returns>
        EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Find Entity Entry
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="entity">Entity To Find Entry</param>
        /// <returns>Entity Entry</returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}

using eBroker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace eBroker.DAL
{
    /// <summary>
    /// Broker Context Class
    /// </summary>
    public class BrokerContext: DbContext, IBrokerContext
    {
        /// <summary>
        /// Initalizes a new instance of the Broker Context
        /// </summary>
        /// <param name="options">Database Context Options</param>
        public BrokerContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Traders Datasets
        /// </summary>
        public DbSet<Trader> Traders { get; set; }

        /// <summary>
        /// Equities Datasets
        /// </summary>
        public DbSet<Equity> Equities { get; set; }

        /// <summary>
        /// TraderEquities Datasets
        /// </summary>
        public DbSet<TraderEquity> TraderEquities { get; set; }

        /// <summary>
        /// On Model Creating handler
        /// </summary>
        /// <param name="modelBuilder">Model Builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trader>().ToTable("Trader");
            modelBuilder.Entity<Equity>().ToTable("Equity");
            modelBuilder.Entity<TraderEquity>().ToTable("TraderEquity");
        }
    }
}

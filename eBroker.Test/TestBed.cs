using System.Collections.Generic;
using eBroker.DAL;
using eBroker.Model;
using Microsoft.EntityFrameworkCore;

namespace eBroker.Test
{
    /// <summary>
    /// Test Bed
    /// </summary>
    public class TestBed
    {
        /// <summary>
        /// Get Database Options
        /// </summary>
        /// <returns>Database Context Options</returns>
        public static DbContextOptions GetDbOptions()
        {
            var options = new DbContextOptionsBuilder<BrokerContext>().UseInMemoryDatabase(databaseName: "TraderDatabase").Options;

            using (var context = new BrokerContext(options))
            {
                var sensex = new Equity { Name = "Sensex", Amount = 57788.03 };
                var nifty = new Equity { Name = "Nifty", Amount = 17221.40 };
                var equities = new Equity[]
                {
                    sensex,
                    nifty
                };

                foreach (Equity e in equities)
                {
                    context.Equities.Add(e);
                }

                var jitesh = new Trader { Name = "Jitesh", TraderEquities = new List<TraderEquity>() { }, Funds = 70000 };
                var neha = new Trader { Name = "Neha", TraderEquities = new List<TraderEquity>() { }, Funds = 80000 };
                var traders = new Trader[] {
                    jitesh,
                    neha
                };

                foreach (Trader t in traders)
                {
                    context.Traders.Add(t);
                }

                context.SaveChanges();

                var traderEquity = new TraderEquity { TraderId = jitesh.ID, Quantity = 10, Equity = sensex };

                context.TraderEquities.Add(traderEquity);
                jitesh.TraderEquities.Add(traderEquity);
                context.Entry(jitesh).State = EntityState.Modified;

                context.SaveChanges();
            }

            return options;
        }
    }
}

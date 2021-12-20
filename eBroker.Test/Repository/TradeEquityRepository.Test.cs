using System.Collections.Generic;
using System.Linq;
using eBroker.DAL;
using eBroker.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eBroker.Test
{
    /// <summary>
    /// TradeRepository Test Class
    /// </summary>
    public class TradeEquityRepositoryTest
    {
        /// <summary>
        /// Database Context Options
        /// </summary>
        DbContextOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeEquityRepositoryTest"/> class.
        /// </summary>
        public TradeEquityRepositoryTest()
        {
            this.options = TestBed.GetDbOptions();
        }

        /// <summary>
        /// Fetch Trader Equity. Verify Successfully Fetch Scenario.
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <param name="quantity">Expected Quantity</param>
        [Theory]
        [InlineData(1, 10)]
        public void FetchTraderEquity_Success(int id, int quantity)
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderEquityRepository traderEquityRepository = new TraderEquityRepository(context);

                // ACT
                var trader = traderEquityRepository.Get(id);

                // ASSERT
                Assert.Equal(quantity, trader.Quantity);
            }
        }

        /// <summary>
        /// Fetch Trader Equity. Verify trader does not exists scenario.
        /// </summary>
        [Fact]
        public void FetchTraderEquity_Failure()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderEquityRepository traderEquityRepository = new TraderEquityRepository(context);

                // ACT
                var trader = traderEquityRepository.Get(1000);

                // ASSERT
                Assert.Null(trader);
            }
        }

        /// <summary>
        /// Verify Fetch all trader equities.
        /// </summary>
        [Fact]
        public void FetchAllTraderEquities()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderEquityRepository traderEquityRepository = new TraderEquityRepository(context);

                // ACT
                var traders = traderEquityRepository.GetAll();

                // ASSERT
                Assert.Equal(10, traders.First().Quantity);
            }
        }

        /// <summary>
        /// Insert Trader Equity. Verify Insertion.
        /// </summary>
        [Fact]
        public void InsertTrader()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderEquityRepository traderEquityRepository = new TraderEquityRepository(context);
                var jitesh = context.Traders.Find(1);
                var sensex = context.Equities.Find(1);
                var trader = new TraderEquity { TraderId = jitesh.ID, Quantity = 50, Equity = sensex };

                // ACT
                var id = traderEquityRepository.Insert(trader);
                var savedtrader = context.TraderEquities.Find(id);

                // ASSERT
                Assert.Equal(50, savedtrader.Quantity);
            }
        }

        /// <summary>
        /// Update Entity. Verify updated amount.
        /// </summary>
        [Fact]
        public void UpdateTrader()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderEquityRepository tradeRepository = new TraderEquityRepository(context);

                var savedtrader = context.TraderEquities.Find(1);
                savedtrader.Quantity = 15;

                // ACT
                tradeRepository.Update(savedtrader);
                var updatedTrader = context.TraderEquities.Find(1);

                // ASSERT
                Assert.Equal(15, updatedTrader.Quantity);
            }
        }

        /// <summary>
        /// Delete Trader Equity
        /// </summary>
        [Fact]
        public void DeleteTrader()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderEquityRepository tradeRepository = new TraderEquityRepository(context);

                var savedtrader = context.TraderEquities.Last().ID;

                // ACT
                tradeRepository.Delete(savedtrader);
                var updatedTrader = context.TraderEquities.Find(savedtrader);

                // ASSERT
                Assert.Null(updatedTrader);
            }
        }

        /// <summary>
        /// Dispose Respository
        /// </summary>
        [Fact]
        public void DisposeReleaseMemory()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderEquityRepository tradeRepository = new TraderEquityRepository(context);

                // ACT
                tradeRepository.Dispose();

                // ASSERT
                Assert.True(tradeRepository.DisposedValue);
            }
        }
    }
}

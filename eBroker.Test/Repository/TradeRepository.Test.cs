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
    public class TradeRepositoryTest
    {
        /// <summary>
        /// Database Context Options
        /// </summary>
        DbContextOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="TradeRepositoryTest"/> class.
        /// </summary>
        public TradeRepositoryTest()
        {
            this.options = TestBed.GetDbOptions();
        }

        /// <summary>
        /// Fetch Trader. Verify Successfully Fetch Scenario.
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <param name="expectedName">Expected Name</param>
        [Theory]
        [InlineData(1, "Jitesh")]
        public void FetchTrader_Success(int id, string expectedName)
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderRepository tradeRepository = new TraderRepository(context);

                // ACT
                var trader = tradeRepository.Get(id);

                // ASSERT
                Assert.Equal(expectedName, trader.Name);
            }
        }

        /// <summary>
        /// Fetch Trader. Verify trader does not exists scenario.
        /// </summary>
        [Fact]
        public void FetchTrader_Failure()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderRepository tradeRepository = new TraderRepository(context);

                // ACT
                var trader = tradeRepository.Get(1000);

                // ASSERT
                Assert.Null(trader);
            }
        }

        /// <summary>
        /// Verify Fetch all traders.
        /// </summary>
        [Fact]
        public void FetchAllTrader()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderRepository tradeRepository = new TraderRepository(context);

                // ACT
                var traders = tradeRepository.GetAll();

                // ASSERT
                Assert.Equal("Jitesh", traders.First().Name);
                Assert.Equal("Neha", traders.Last().Name);
            }
        }

        /// <summary>
        /// Insert Trader. Verify Insertion.
        /// </summary>
        /// <param name="funds">Amount with trader</param>
        /// <param name="name">Name of trader</param>
        [Theory]
        [InlineData(8000, "Hari")]
        [InlineData(9000, "Kiran")]
        public void InsertTrader(double funds, string name)
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderRepository tradeRepository = new TraderRepository(context);
                var trader = new Trader { Funds = funds, Name = name, TraderEquities = new List<TraderEquity>() };

                // ACT
                var id = tradeRepository.Insert(trader);
                var savedtrader = context.Traders.Find(id);

                // ASSERT
                Assert.Equal(name, savedtrader.Name);
                Assert.Equal(funds, savedtrader.Funds);
            }
        }

        /// <summary>
        /// Update Entity. Verify updated amount.
        /// </summary>
        /// <param name="id">Identity of trader</param>
        /// <param name="amount">Amount to update</param>
        [Theory]
        [InlineData(1, 20000)]
        [InlineData(2, 40000)]
        public void UpdateTrader(int id, double amount)
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderRepository tradeRepository = new TraderRepository(context);
                var savedtrader = context.Traders.Find(id);
                savedtrader.Funds = amount;

                // ACT
                tradeRepository.Update(savedtrader);
                var updatedTrader = context.Traders.Find(id);

                // ASSERT
                Assert.Equal(amount, updatedTrader.Funds);
            }
        }

        /// <summary>
        /// Delete Trader
        /// </summary>
        [Fact]
        public void DeleteTrader()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                TraderRepository tradeRepository = new TraderRepository(context);
                var savedtrader = context.Traders.Last().ID;

                // ACT
                tradeRepository.Delete(savedtrader);
                var updatedTrader = context.Traders.Find(savedtrader);

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
                TraderRepository tradeRepository = new TraderRepository(context);

                // ACT
                tradeRepository.Dispose();

                // ASSERT
                Assert.True(tradeRepository.DisposedValue);
            }
        }
    }
}

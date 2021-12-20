using System.Collections.Generic;
using System.Linq;
using eBroker.DAL;
using eBroker.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace eBroker.Test
{
    /// <summary>
    /// Equity Repository Test Class
    /// </summary>
    public class EquityRepositoryTest
    {
        /// <summary>
        /// Database Context Options
        /// </summary>
        DbContextOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquityRepositoryTest"/> class.
        /// </summary>
        public EquityRepositoryTest()
        {
            this.options = TestBed.GetDbOptions();
        }

        /// <summary>
        /// Fetch Equity. Verify Successfully Fetch Scenario.
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <param name="expectedName">Expected Name</param>
        [Theory]
        [InlineData(1, "Sensex")]
        public void FetchEquity_Success(int id, string expectedName)
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                EquityRepository equityRepository = new EquityRepository(context);

                // ACT
                var equity = equityRepository.Get(id);

                // ASSERT
                Assert.Equal(expectedName, equity.Name);
            }
        }

        /// <summary>
        /// Fetch Equity. Verify Equity does not exists scenario.
        /// </summary>
        [Fact]
        public void FetchEquity_Failure()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                EquityRepository equityRepository = new EquityRepository(context);

                // ACT
                var equity = equityRepository.Get(1000);

                // ASSERT
                Assert.Null(equity);
            }
        }

        /// <summary>
        /// Verify Fetch all Equities.
        /// </summary>
        [Fact]
        public void FetchAllEquity()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                EquityRepository equityRepository = new EquityRepository(context);

                // ACT
                var equities = equityRepository.GetAll();

                // ASSERT
                Assert.Equal("Sensex", equities.First().Name);
                Assert.Equal("Nifty", equities.Last().Name);
            }
        }

        /// <summary>
        /// Insert Equity. Verify Insertion.
        /// </summary>
        /// <param name="amount">Amount of Equity</param>
        /// <param name="name">Name of Equity</param>
        [Theory]
        [InlineData(8000, "Sensex2")]
        [InlineData(9000, "Nifty2")]
        public void InsertEquity(double amount, string name)
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                EquityRepository equityRepository = new EquityRepository(context);
                var equity = new Equity { Amount = amount, Name = name };

                // ACT
                var id = equityRepository.Insert(equity);
                var savedEquity = context.Equities.Find(id);

                // ASSERT
                Assert.Equal(name, savedEquity.Name);
                Assert.Equal(amount, savedEquity.Amount);
            }
        }

        /// <summary>
        /// Update Entity. Verify updated amount.
        /// </summary>
        /// <param name="id">Identity of Equity</param>
        /// <param name="amount">Amount to update</param>
        [Theory]
        [InlineData(1, 20000)]
        [InlineData(2, 40000)]
        public void UpdateEquity(int id, double amount)
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                EquityRepository equityRepository = new EquityRepository(context);
                var savedEquity = context.Equities.Find(id);
                savedEquity.Amount = amount;

                // ACT
                equityRepository.Update(savedEquity);
                var updatedEntity = context.Equities.Find(id);

                // ASSERT
                Assert.Equal(amount, updatedEntity.Amount);
            }
        }

        /// <summary>
        /// Delete Equity
        /// </summary>
        [Fact]
        public void DeleteEquity()
        {
            // ARRANGE
            using (var context = new BrokerContext(options))
            {
                EquityRepository tradeRepository = new EquityRepository(context);
                var savedEquity = context.Equities.Last().ID;

                // ACT
                tradeRepository.Delete(savedEquity);
                var updatedEntity = context.Equities.Find(savedEquity);

                // ASSERT
                Assert.Null(updatedEntity);
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
                EquityRepository equityRepository = new EquityRepository(context);

                // ACT
                equityRepository.Dispose();
                
                // ASSERT
                Assert.True(equityRepository.DisposedValue);
            }
        }
    }
}

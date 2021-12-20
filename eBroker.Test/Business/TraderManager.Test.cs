using System;
using System.Collections.Generic;
using System.Linq;
using eBroker.Business;
using eBroker.Core;
using eBroker.DAL;
using eBroker.Model;
using Moq;
using Xunit;

namespace eBroker.Test
{
    /// <summary>
    /// Trader Manager Test Class
    /// </summary>
    public class TraderManagerTest
    {
        /// <summary>
        /// Trader Manager (System Under Test)
        /// </summary>
        ITraderManager traderManager;

        /// <summary>
        /// Repository Mock
        /// </summary>
        Mock<IRepository<Trader>> repositoryMock;

        /// <summary>
        /// Equity Repository Mock
        /// </summary>
        Mock<IRepository<Equity>> equityRepositoryMock;

        /// <summary>
        /// Operations Utility Mock
        /// </summary>
        Mock<IOperationsUtilityProxy> operationsUtilityProxyMock;

        /// <summary>
        /// Test Fixture
        /// </summary>
        public TraderManagerTest()
        {
            this.repositoryMock = new Mock<IRepository<Trader>>();
            this.equityRepositoryMock = new Mock<IRepository<Equity>>();
            this.operationsUtilityProxyMock = new Mock<IOperationsUtilityProxy>();
            this.traderManager = new TraderManager(this.repositoryMock.Object, this.equityRepositoryMock.Object, this.operationsUtilityProxyMock.Object);
        }

        /// <summary>
        /// Get All Models
        /// </summary>
        [Fact]
        public void GetAll_Success()
        {
            // ARRANGE
            var traders = new List<Trader>()
            {
                new Trader { ID = 1, Name = "Jitesh", Funds = 20000 },
                new Trader { ID = 1, Name = "Neha", Funds = 30000 }
            };

            this.repositoryMock.Setup(x => x.GetAll()).Returns(traders);
            
            // ACT
            var result = this.traderManager.GetAll();
            
            // ASSERT
            this.repositoryMock.Verify(x => x.GetAll(), Times.Once);
            for (int i = 0; i < result.Count(); i++)
            {
                Assert.Equal(traders.ElementAt(i).ID, result.ElementAt(i).ID);
                Assert.Equal(traders.ElementAt(i).Name, result.ElementAt(i).Name);
                Assert.Equal(traders.ElementAt(i).Funds, result.ElementAt(i).Funds);
            }
        }

        /// <summary>
        /// Get Not available Model. Verify Null Handling.
        /// </summary>
        [Fact]
        public void GetAll_NullValues()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.GetAll());
            
            // ACT
            var result = this.traderManager.GetAll();
            
            // ASSERT
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="id">Model Id</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(1000)]
        public void Get_Success(int id)
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { ID = id });
            
            // ACT
            var result = this.traderManager.GetById(id);
            
            // ASSERT
            Assert.Equal(id, result.ID);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == id)), Times.Once);
        }

        /// <summary>
        /// Get Model. Null Handling
        /// </summary>
        [Fact]
        public void Get_Null()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>()));
            
            // ACT
            var result = this.traderManager.GetById(1);
            
            // ASSERT
            Assert.Null(result);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
        }

        /// <summary>
        /// Get Model. Invalid Argument handling for Identifier.
        /// </summary>
        /// <param name="id">Model Identifier</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Get_InvalidArgument(int id)
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>()));
            
            // ACT, ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => this.traderManager.GetById(id));
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == id)), Times.Never);
        }

        /// <summary>
        /// Insert Model. Success Hanlders.
        /// </summary>
        /// <param name="name">Model Name</param>
        /// <param name="amount">Model Amount</param>
        /// <param name="id">Model Id</param>
        [Theory, ClassData(typeof(TraderDataSource))]
        public void Insert_Success(string name, double amount, int id)
        {
            // ARRANGE
            var trader = new Trader { Name = name, Funds = amount };
            this.repositoryMock.Setup(x => x.Insert(It.IsAny<Trader>())).Returns(id);
            
            // ACT
            var result = this.traderManager.Insert(trader);
            
            // ASSERT
            Assert.Equal(id, result);
            this.repositoryMock.Verify(x => x.Insert(It.Is<Trader>(x => x == trader)), Times.Once);
        }

        /// <summary>
        /// Insert Model. Null Exception Handling.
        /// </summary>
        [Fact]
        public void Insert_NullException()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Insert(It.IsAny<Trader>()));
            
            // ACT, ARRANGE
            Assert.Throws<NullReferenceException>(() => this.traderManager.Insert(null));
            this.repositoryMock.Verify(x => x.Insert(It.Is<Trader>(x => x == null)), Times.Never);
        }

        /// <summary>
        /// Update Model. Success Hanlder.
        /// </summary>
        /// <param name="name">Model Name</param>
        /// <param name="amount">Model Amount</param>
        /// <param name="id">Model Id</param>
        [Theory, ClassData(typeof(TraderDataSource))]
        public void Update_Success(string name, double amount, int id)
        {
            // ARRANGE
            var trader = new Trader { ID = id, Name = name, Funds = amount };
            this.repositoryMock.Setup(x => x.Update(It.IsAny<Trader>()));
            
            // ACT
            this.traderManager.Update(trader);
            
            // ASSERT
            this.repositoryMock.Verify(x => x.Update(It.Is<Trader>(x => x == trader)), Times.Once);
        }

        /// <summary>
        /// Update Model. Null Handler.
        /// </summary>
        [Fact]
        public void Update_NullException()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Update(It.IsAny<Trader>()));
            
            // ACT, ASSERT
            Assert.Throws<NullReferenceException>(() => this.traderManager.Update(null));
            this.repositoryMock.Verify(x => x.Update(It.Is<Trader>(x => x == null)), Times.Never);
        }

        /// <summary>
        /// Delete Model. Invalid Argument Handler.
        /// </summary>
        /// <param name="id">Model Id</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Delete_InvalidArgument(int id)
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));
            
            // ACT, ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => this.traderManager.Delete(id));
            this.repositoryMock.Verify(x => x.Delete(It.Is<int>(x => x == id)), Times.Never);
        }

        /// <summary>
        /// Delete Model. Success Handler
        /// </summary>
        /// <param name="id">Model Id</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void Delete_Success(int id)
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Delete(It.IsAny<int>()));
            
            // ACT
            this.traderManager.Delete(id);
            
            // ASSERT
            this.repositoryMock.Verify(x => x.Delete(It.Is<int>(x => x == id)), Times.Once);
        }

        /// <summary>
        /// Buy Equity Outside Working Hours
        /// </summary>
        [Fact]
        public void BuyEquity_OutsideWorkingHours()
        {
            // ARRANGE
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(false);
            var time = new DateTime(2121, 12, 18, 8, 59, 0);

            // ACT
            var status = this.traderManager.BuyEquity(1, 1, 5, time);

            // ASSERT
            Assert.False(status);
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Never);
        }

        /// <summary>
        /// Buy Equity out of funds
        /// </summary>
        [Fact]
        public void BuyEquity_NoFunds()
        {
            // ARRANGE
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(true);
            this.operationsUtilityProxyMock.Setup(x => x.AddEquities(It.IsAny<Trader>(), It.IsAny<Equity>(), It.IsAny<int>())).Returns(false);
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { ID = 1, Name = "Jitesh", Funds = 80000, TraderEquities = new List<TraderEquity>() });
            this.equityRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity { ID = 1, Name = "Sensex", Amount = 50000 });
            var time = new DateTime(2121, 12, 18, 9, 0, 0);

            // ACT
            var status = this.traderManager.BuyEquity(1, 1, 5, time);

            // ASSERT
            Assert.False(status);
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.equityRepositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.operationsUtilityProxyMock.Verify(x => x.AddEquities(It.IsAny<Trader>(), It.IsAny<Equity>(), It.IsAny<int>()), Times.Once);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }

        /// <summary>
        /// Buy Equity. Success Handler.
        /// </summary>
        [Fact]
        public void BuyEquity_Success()
        {
            // ARRANGE
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(true);
            this.operationsUtilityProxyMock.Setup(x => x.AddEquities(It.IsAny<Trader>(), It.IsAny<Equity>(), It.IsAny<int>())).Returns(true);
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { ID = 1, Name = "Jitesh", Funds = 80000, TraderEquities = new List<TraderEquity>() });
            this.equityRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity { ID = 1, Name = "Sensex", Amount = 50000 });
            this.repositoryMock.Setup(x => x.Update(It.IsAny<Trader>()));
            var time = new DateTime(2121, 12, 18, 9, 0, 0);

            // ACT
            var status = this.traderManager.BuyEquity(1, 1, 5, time);

            // ASSERT
            Assert.True(status);
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.equityRepositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.operationsUtilityProxyMock.Verify(x => x.AddEquities(It.IsAny<Trader>(), It.IsAny<Equity>(), It.IsAny<int>()), Times.Once);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Once);
        }

        /// <summary>
        /// Buy Equity Trader Not Found
        /// </summary>
        [Fact]
        public void BuyEquity_TraderNotFound()
        {
            // ARRANGE
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(true);
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>()));
            var time = new DateTime(2121, 12, 18, 9, 0, 0);

            // ASSERT, ACT
            Assert.Throws<RecordNotFoundException>(() => this.traderManager.BuyEquity(1, 1, 5, time));
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.equityRepositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Never);
            this.operationsUtilityProxyMock.Verify(x => x.AddEquities(It.IsAny<Trader>(), It.IsAny<Equity>(), It.IsAny<int>()), Times.Never);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }

        /// <summary>
        /// Buy Equity. Equity Not Found.
        /// </summary>
        [Fact]
        public void BuyEquity_EquityNotFound()
        {
            // ARRANGE
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(true);
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { ID = 1, Name = "Jitesh", Funds = 80000, TraderEquities = new List<TraderEquity>() });
            this.equityRepositoryMock.Setup(x => x.Get(It.IsAny<int>()));
            var time = new DateTime(2121, 12, 18, 9, 0, 0);

            // ACT, ASSERT
            Assert.Throws<RecordNotFoundException>(() => this.traderManager.BuyEquity(1, 1, 5, time));
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.equityRepositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.operationsUtilityProxyMock.Verify(x => x.AddEquities(It.IsAny<Trader>(), It.IsAny<Equity>(), It.IsAny<int>()), Times.Never);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }

        /// <summary>
        /// Sell Equity outside working hours
        /// </summary>
        [Fact]
        public void SellEquity_OutsideWorkingHours()
        {
            // ARRANGE
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(false);
            var time = new DateTime(2121, 12, 18, 8, 59, 0);

            // ACT
            var status = this.traderManager.SellEquity(1, 1, 5, time);

            // ASSERT
            Assert.False(status);
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Never);
        }

        /// <summary>
        /// Sell Equity out of quantity
        /// </summary>
        [Fact]
        public void SellEquity_NoQuantity()
        {
            // ARRANGE
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(true);
            this.operationsUtilityProxyMock.Setup(x => x.ReduceEquities(It.IsAny<Trader>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { ID = 1, Name = "Jitesh", Funds = 80000, TraderEquities = new List<TraderEquity>() });
            var time = new DateTime(2121, 12, 18, 9, 0, 0);

            // ACT
            var status = this.traderManager.SellEquity(1, 1, 5, time);

            // ASSERT
            Assert.False(status);
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.operationsUtilityProxyMock.Verify(x => x.ReduceEquities(It.IsAny<Trader>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }

        /// <summary>
        /// Sell Equity. Success Handler
        /// </summary>
        [Fact]
        public void SellEquity_Success()
        {
            // ARRANGe
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(true);
            this.operationsUtilityProxyMock.Setup(x => x.ReduceEquities(It.IsAny<Trader>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { ID = 1, Name = "Jitesh", Funds = 80000, TraderEquities = new List<TraderEquity>() });
            this.repositoryMock.Setup(x => x.Update(It.IsAny<Trader>()));
            var time = new DateTime(2121, 12, 18, 9, 0, 0);

            // ACT
            var status = this.traderManager.SellEquity(1, 1, 5, time);

            // ASSERT
            Assert.True(status);
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.operationsUtilityProxyMock.Verify(x => x.ReduceEquities(It.IsAny<Trader>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Once);
        }

        /// <summary>
        /// Sell Equity. Trader Not Found.
        /// </summary>
        [Fact]
        public void SellEquity_TraderNotFound()
        {
            // ARRANGE
            this.operationsUtilityProxyMock.Setup(x => x.IsOperatingHours(It.IsAny<DateTime>())).Returns(true);
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>()));
            var time = new DateTime(2121, 12, 18, 9, 0, 0);

            // ACT, ASSERT
            Assert.Throws<RecordNotFoundException>(() => this.traderManager.SellEquity(1, 1, 5, time));
            this.operationsUtilityProxyMock.Verify(x => x.IsOperatingHours(It.IsAny<DateTime>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.operationsUtilityProxyMock.Verify(x => x.ReduceEquities(It.IsAny<Trader>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }

        /// <summary>
        /// Add Funds. Trader Not Found.
        /// </summary>
        [Fact]
        public void AddFunds_TraderNotFound()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>()));

            // ACT, ASSERT
            Assert.Throws<RecordNotFoundException>(() => this.traderManager.AddFunds(1, 10000));
            this.operationsUtilityProxyMock.Verify(x => x.IncreaseFunds(It.IsAny<Trader>(), It.IsAny<double>()), Times.Never);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }

        /// <summary>
        /// Add Funds. Unsuccessful Handler.
        /// </summary>
        [Fact]
        public void AddFunds_Unsuccessful()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { ID = 1, Name = "Jitesh", Funds = 80000, TraderEquities = new List<TraderEquity>() });
            this.operationsUtilityProxyMock.Setup(x => x.IncreaseFunds(It.IsAny<Trader>(), It.IsAny<double>())).Returns(false);

            // ACT
            var status = this.traderManager.AddFunds(1, 10000);

            // ASSERT
            Assert.False(status);
            this.operationsUtilityProxyMock.Verify(x => x.IncreaseFunds(It.IsAny<Trader>(), It.IsAny<double>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }

        /// <summary>
        /// Add Funds. Success Handler.
        /// </summary>
        [Fact]
        public void AddFunds_Successful()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Trader { ID = 1, Name = "Jitesh", Funds = 80000, TraderEquities = new List<TraderEquity>() });
            this.operationsUtilityProxyMock.Setup(x => x.IncreaseFunds(It.IsAny<Trader>(), It.IsAny<double>())).Returns(true);

            // ACT
            var status = this.traderManager.AddFunds(1, 10000);

            // ASSERT
            Assert.True(status);
            this.operationsUtilityProxyMock.Verify(x => x.IncreaseFunds(It.IsAny<Trader>(), It.IsAny<double>()), Times.Once);
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == 1)), Times.Once);
            this.repositoryMock.Verify(x => x.Update(It.IsAny<Trader>()), Times.Once);
        }
    }
}
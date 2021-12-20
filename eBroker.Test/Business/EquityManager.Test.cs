using System;
using System.Collections.Generic;
using System.Linq;
using eBroker.Business;
using eBroker.DAL;
using eBroker.Model;
using Moq;
using Xunit;

namespace eBroker.Test
{
    /// <summary>
    /// Equity Manager Test Class
    /// </summary>
    public class EquityManagerTest
    {
        /// <summary>
        /// Equity Manager (System Under Test)
        /// </summary>
        IManager<Equity> equityManager;

        /// <summary>
        /// Repository Mock
        /// </summary>
        Mock<IRepository<Equity>> repositoryMock;

        /// <summary>
        /// Test Fixture
        /// </summary>
        public EquityManagerTest()
        {
            this.repositoryMock = new Mock<IRepository<Equity>>();
            this.equityManager = new EquityManager(repositoryMock.Object);
        }

        /// <summary>
        /// Get All Models
        /// </summary>
        [Fact]
        public void GetAll_Success()
        {
            // ARRANGE
            var equities = new List<Equity>()
            {
                new Equity { ID = 1, Name = "Sensex", Amount = 57788.03 },
                new Equity { ID = 1, Name = "Nifty", Amount = 17221.40 }
            };

            this.repositoryMock.Setup(x => x.GetAll()).Returns(equities);
            
            // ACT
            var result = this.equityManager.GetAll();
            
            // ASSERT
            this.repositoryMock.Verify(x => x.GetAll(), Times.Once);
            for (int i = 0; i < result.Count(); i++)
            {
                Assert.Equal(equities.ElementAt(i).ID, result.ElementAt(i).ID);
                Assert.Equal(equities.ElementAt(i).Name, result.ElementAt(i).Name);
                Assert.Equal(equities.ElementAt(i).Amount, result.ElementAt(i).Amount);
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
            var result = this.equityManager.GetAll();
            
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
            this.repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Equity { ID = id });
            
            // ACT
            var result = this.equityManager.GetById(id);
            
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
            var result = this.equityManager.GetById(1);
            
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
            Assert.Throws<ArgumentOutOfRangeException>(() => this.equityManager.GetById(id));
            this.repositoryMock.Verify(x => x.Get(It.Is<int>(x => x == id)), Times.Never);
        }

        /// <summary>
        /// Insert Model. Success Hanlders.
        /// </summary>
        /// <param name="name">Model Name</param>
        /// <param name="amount">Model Amount</param>
        /// <param name="id">Model Id</param>
        [Theory, ClassData(typeof(EquityDataSource))]
        public void Insert_Success(string name, double amount, int id)
        {
            // ARRANGE
            var equity = new Equity { Name = name, Amount = amount };
            this.repositoryMock.Setup(x => x.Insert(It.IsAny<Equity>())).Returns(id);
            
            // ACT
            var result = this.equityManager.Insert(equity);
            
            // ASSERT
            Assert.Equal(id, result);
            this.repositoryMock.Verify(x => x.Insert(It.Is<Equity>(x => x == equity)), Times.Once);
        }

        /// <summary>
        /// Insert Model. Null Exception Handling.
        /// </summary>
        [Fact]
        public void Insert_NullException()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Insert(It.IsAny<Equity>()));
            
            // ACT, ARRANGE
            Assert.Throws<NullReferenceException>(() => this.equityManager.Insert(null));
            this.repositoryMock.Verify(x => x.Insert(It.Is<Equity>(x => x == null)), Times.Never);
        }

        /// <summary>
        /// Update Model. Success Hanlder.
        /// </summary>
        /// <param name="name">Model Name</param>
        /// <param name="amount">Model Amount</param>
        /// <param name="id">Model Id</param>
        [Theory, ClassData(typeof(EquityDataSource))]
        public void Update_Success(string name, double amount, int id)
        {
            // ARRANGE
            var equity = new Equity { ID = id, Name = name, Amount = amount };
            this.repositoryMock.Setup(x => x.Update(It.IsAny<Equity>()));
            
            // ACT
            this.equityManager.Update(equity);
            
            // ASSERT
            this.repositoryMock.Verify(x => x.Update(It.Is<Equity>(x => x == equity)), Times.Once);
        }

        /// <summary>
        /// Update Model. Null Handler.
        /// </summary>
        [Fact]
        public void Update_NullException()
        {
            // ARRANGE
            this.repositoryMock.Setup(x => x.Update(It.IsAny<Equity>()));
            
            // ACT, ASSERT
            Assert.Throws<NullReferenceException>(() => this.equityManager.Update(null));
            this.repositoryMock.Verify(x => x.Update(It.Is<Equity>(x => x == null)), Times.Never);
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
            Assert.Throws<ArgumentOutOfRangeException>(() => this.equityManager.Delete(id));
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
            this.equityManager.Delete(id);
            
            // ASSERT
            this.repositoryMock.Verify(x => x.Delete(It.Is<int>(x => x == id)), Times.Once);
        }
    }
}
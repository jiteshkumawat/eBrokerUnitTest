using eBroker.Business;
using eBroker.Core;
using eBroker.Model;
using eBroker.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace eBroker.Test.Controller
{
    /// <summary>
    /// Trader Controller Test
    /// </summary>
    public class TraderControllerTest
    {
        /// <summary>
        /// Trader Controller (System Under Test)
        /// </summary>
        TraderController traderController;

        /// <summary>
        /// Trader Manager Mock
        /// </summary>
        Mock<ITraderManager> traderManager;

        /// <summary>
        /// Test Setup
        /// </summary>
        public TraderControllerTest()
        {
            this.traderManager = new Mock<ITraderManager>();
            this.traderController = new TraderController(this.traderManager.Object);
        }

        /// <summary>
        /// Ok Result for Get All.
        /// </summary>
        [Fact]
        public void Get_ReturnsOkResult()
        {
            // ACT
            var okResult = this.traderController.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
            this.traderManager.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Validate items for Get All.
        /// </summary>
        [Fact]
        public void Get_ResturnsAllItems()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.GetAll()).Returns(new List<Trader>() { new Trader { ID = 1, Name = "Jitesh" }, new Trader { ID = 2, Name = "Neha" } });
            
            // ACT
            var okResult = this.traderController.Get() as OkObjectResult;

            // ASSERT
            var items = Assert.IsAssignableFrom<IEnumerable<Trader>>(okResult.Value);
            Assert.Equal(2, items.Count());
            this.traderManager.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Not Found for Get.
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void GetById_ReturnsNotFoundResult(int id)
        {
            // ACT
            var result = this.traderController.Get(id);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
            this.traderManager.Verify(x => x.GetById(It.Is<int>(i => i == id)), Times.Once);
        }

        /// <summary>
        /// Bad Request for Get.
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void GetById_ReturnsBadRequest(int id)
        {
            // ARRANGE
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>())).Throws(new ArgumentOutOfRangeException());
            
            // ACT
            var result = this.traderController.Get(id);

            // ASSERT
            Assert.IsType<BadRequestResult>(result);
            this.traderManager.Verify(x => x.GetById(It.Is<int>(i => i == id)), Times.Once);
        }

        /// <summary>
        /// Ok Result for Get.
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void GetById_ReturnsOkResult(int id)
        {
            // ARRANGE
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Trader { ID = id });
            
            // ACT
            var result = this.traderController.Get(id) as OkObjectResult;

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Trader>(result.Value);
            Assert.Equal(id, (result.Value as Trader).ID);
            this.traderManager.Verify(x => x.GetById(It.Is<int>(i => i == id)), Times.Once);
        }

        /// <summary>
        /// Created Response for Post
        /// </summary>
        [Fact]
        public void Post_ReturnsCreatedResponse()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.Insert(It.IsAny<Trader>())).Returns(1);
            var trader = new Trader { Name = "Jitesh", Funds = 30000 };

            // ACT
            var createdResponse = this.traderController.Post(trader);
            
            // ASSERT
            Assert.IsType<CreatedAtActionResult>(createdResponse);
            this.traderManager.Verify(x => x.Insert(It.IsAny<Trader>()), Times.Once);
        }

        /// <summary>
        /// Items created by Post
        /// </summary>
        [Fact]
        public void Post_ReturnedResponseHasCreatedItem()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.Insert(It.IsAny<Trader>())).Returns(1);
            var trader = new Trader { Name = "Jitesh", Funds = 30000 };

            // ACT
            var createdResponse = this.traderController.Post(trader) as CreatedAtActionResult;
            var item = createdResponse.Value as Trader;

            // ASSERT
            Assert.IsType<Trader>(item);
            Assert.Equal(1, trader.ID);
            this.traderManager.Verify(x => x.Insert(It.IsAny<Trader>()), Times.Once);
        }

        /// <summary>
        /// Not Found for Put
        /// </summary>
        [Fact]
        public void Put_ReturnsNotFoundResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>()));
            var trader = new Trader { Name = "Jitesh", Funds = 30000, ID = 1 };

            // ACT
            var result = this.traderController.Put(trader);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
            this.traderManager.Verify(x => x.GetById(It.Is<int>(id => id == 1)), Times.Once);
            this.traderManager.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }

        /// <summary>
        /// Bad Request for Put
        /// </summary>
        [Fact]
        public void Put_ReturnsBadRequestResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>())).Throws(new ArgumentOutOfRangeException());
            var trader = new Trader { Name = "Jitesh", Funds = 30000, ID = 1 };

            // ACT
            var result = this.traderController.Put(trader);

            // ASSERT
            Assert.IsType<BadRequestResult>(result);
            this.traderManager.Verify(x => x.GetById(It.Is<int>(id => id == 1)), Times.Once);
            this.traderManager.Verify(x => x.Update(It.IsAny<Trader>()), Times.Never);
        }
        
        /// <summary>
        /// Ok Result for Put
        /// </summary>
        [Fact]
        public void Put_ReturnsOkResult()
        {
            // ARRANGE
            var trader = new Trader { Name = "Jitesh", Funds = 30000, ID = 1 };
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>())).Returns(trader);
            this.traderManager.Setup(x => x.Update(It.IsAny<Trader>()));

            // ACT
            var result = this.traderController.Put(trader);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
            this.traderManager.Verify(x => x.GetById(It.Is<int>(id => id == 1)), Times.Once);
            this.traderManager.Verify(x => x.Update(It.IsAny<Trader>()), Times.Once);
        }

        /// <summary>
        /// Put Update Items.
        /// </summary>
        [Fact]
        public void Put_ReturnsUpdatedItems()
        {
            // ARRANGE
            var trader = new Trader { Name = "Jitesh", Funds = 30000, ID = 1 };
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>())).Returns(trader);
            this.traderManager.Setup(x => x.Update(It.IsAny<Trader>()));

            // ACT
            var result = this.traderController.Put(trader) as OkObjectResult;
            var updatedItem = result.Value as Trader;

            // ASSERT
            Assert.IsType<Trader>(updatedItem);
            Assert.Equal(trader.ID, updatedItem.ID);
            Assert.Equal(trader.Name, updatedItem.Name);
            Assert.Equal(trader.Funds, updatedItem.Funds);
            this.traderManager.Verify(x => x.GetById(It.Is<int>(id => id == 1)), Times.Once);
            this.traderManager.Verify(x => x.Update(It.IsAny<Trader>()), Times.Once);
        }

        /// <summary>
        /// Bad Request for Delete
        /// </summary>
        [Fact]
        public void Delete_ReturnsBadRequestResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>())).Throws(new ArgumentOutOfRangeException());

            // ACT
            var response = this.traderController.Delete(1);

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
            this.traderManager.Verify(x => x.Delete(It.Is<int>(id => id == 1)), Times.Never);
        }

        /// <summary>
        /// Not Found for Delete
        /// </summary>
        [Fact]
        public void Delete_ReturnsNotFoundResponse()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>()));

            // ACT
            var response = this.traderController.Delete(1);

            // ASSERT
            Assert.IsType<NotFoundResult>(response);
            this.traderManager.Verify(x => x.Delete(It.Is<int>(id => id == 1)), Times.Never);
        }

        /// <summary>
        /// No Content for Delete
        /// </summary>
        [Fact]
        public void Delete_ReturnsNoContentResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Trader { ID = 1 });
            this.traderManager.Setup(x => x.Delete(It.IsAny<int>()));

            // ACT
            var response = this.traderController.Delete(1);

            // ASSERT
            Assert.IsType<NoContentResult>(response);
            this.traderManager.Verify(x => x.Delete(It.Is<int>(id => id == 1)), Times.Once);
        }

        /// <summary>
        /// Bad Request for Buy Equity
        /// </summary>
        [Fact]
        public void BuyEquity_ReturnsBadRequestResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.BuyEquity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Returns(false);

            // ACT
            var response = this.traderController.BuyEquity(1, 1, 10, DateTime.Now);

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
        }

        /// <summary>
        /// Not Found for Buy Equity
        /// </summary>
        /// <param name="entity"></param>
        [Theory]
        [InlineData("Trader")]
        [InlineData("Equity")]
        public void BuyEquity_ReturnsNotFoundResult(string entity)
        {
            // ARRANGE
            this.traderManager.Setup(x => x.BuyEquity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Throws(new RecordNotFoundException() { EntityName = entity });

            // ACT
            var response = this.traderController.BuyEquity(1, 1, 10, DateTime.Now) as NotFoundObjectResult;

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(entity, response.Value);
        }

        /// <summary>
        /// Bad Request for Buy Equity
        /// </summary>
        [Fact]
        public void BuyEquity_ReturnsBadRequestResult_WithInvalidData()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.BuyEquity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Throws(new ArgumentOutOfRangeException());

            // ACT
            var response = this.traderController.BuyEquity(-1, 1, 10, DateTime.Now);

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
        }

        /// <summary>
        /// No Content for Buy Equity
        /// </summary>
        [Fact]
        public void BuyEquity_ReturnsNoContentResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.BuyEquity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Returns(true);

            // ACT
            var response = this.traderController.BuyEquity(1, 1, 10, DateTime.Now);

            // ASSERT
            Assert.IsType<NoContentResult>(response);
        }

        /// <summary>
        /// Bad Request for Sell Equity
        /// </summary>
        [Fact]
        public void SellEquity_ReturnsBadRequestResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Returns(false);

            // ACT
            var response = this.traderController.SellEquity(1, 1, 10, DateTime.Now);

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
        }

        /// <summary>
        /// Not Found for Sell Equity
        /// </summary>
        [Fact]
        public void SellEquity_ReturnsNotFoundResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Throws(new RecordNotFoundException() { EntityName = "Trader" });

            // ACT
            var response = this.traderController.SellEquity(1, 1, 10, DateTime.Now) as NotFoundObjectResult;

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Trader", response.Value);
        }

        /// <summary>
        /// Bad Request for Sell Equity
        /// </summary>
        [Fact]
        public void SellEquity_ReturnsBadRequestResult_WithInvalidData()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Throws(new ArgumentOutOfRangeException());

            // ACT
            var response = this.traderController.SellEquity(-1, 1, 10, DateTime.Now);

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
        }

        /// <summary>
        /// No Content for Sell Equity
        /// </summary>
        [Fact]
        public void SellEquity_ReturnsNoContentResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.SellEquity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>())).Returns(true);

            // ACT
            var response = this.traderController.SellEquity(1, 1, 10, DateTime.Now);

            // ASSERT
            Assert.IsType<NoContentResult>(response);
        }

        /// <summary>
        /// Bad Request for Add Funds
        /// </summary>
        [Fact]
        public void AddFunds_ReturnsBadRequestResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(false);

            // ACT
            var response = this.traderController.AddFunds(1, 10000);

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
        }

        /// <summary>
        /// Not Found for Add Funds
        /// </summary>
        [Fact]
        public void AddFunds_ReturnsNotFoundResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Throws(new RecordNotFoundException() { EntityName = "Trader" });

            // ACT
            var response = this.traderController.AddFunds(1, 10000) as NotFoundObjectResult;

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Trader", response.Value);
        }

        /// <summary>
        /// Bad Request for Add Funds
        /// </summary>
        [Fact]
        public void AddFunds_ReturnsBadRequestResult_WithInvalidData()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Throws(new ArgumentOutOfRangeException());

            // ACT
            var response = this.traderController.AddFunds(-1, 1000);

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
        }

        /// <summary>
        /// No content for Add Funds.
        /// </summary>
        [Fact]
        public void AddFunds_ReturnsNoContentResult()
        {
            // ARRANGE
            this.traderManager.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(true);

            // ACT
            var response = this.traderController.AddFunds(1, 10000);

            // ASSERT
            Assert.IsType<NoContentResult>(response);
        }
    }
}

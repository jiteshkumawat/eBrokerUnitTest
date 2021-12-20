using eBroker.Business;
using eBroker.Model;
using eBroker.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace eBroker.Test.Controller
{
    /// <summary>
    /// Equity Controller Test Class
    /// </summary>
    public class EquityControllerTest
    {
        /// <summary>
        /// Equity Controller (System Under Test)
        /// </summary>
        EquityController equityController;

        /// <summary>
        /// Equity Manager Mock
        /// </summary>
        Mock<IManager<Equity>> equityManager;

        /// <summary>
        /// Test Setup
        /// </summary>
        public EquityControllerTest()
        {
            this.equityManager = new Mock<IManager<Equity>>();
            this.equityController = new EquityController(this.equityManager.Object);
        }

        /// <summary>
        /// OK Result Handler for Get All.
        /// </summary>
        [Fact]
        public void Get_ReturnsOkResult()
        {
            // ACT
            var okResult = this.equityController.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
            this.equityManager.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Validate Items for Get All.
        /// </summary>
        [Fact]
        public void Get_ResturnsAllItems()
        {
            // ARRANGE
            this.equityManager.Setup(x => x.GetAll()).Returns(new List<Equity>() { new Equity { ID = 1, Name = "Sensex" }, new Equity { ID = 2, Name = "Nifty" } });
            
            // ACT
            var okResult = this.equityController.Get() as OkObjectResult;

            // ASSERT
            var items = Assert.IsAssignableFrom<IEnumerable<Equity>>(okResult.Value);
            Assert.Equal(2, items.Count());
            this.equityManager.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// No Record Found for Get.
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void GetById_ReturnsNotFoundResult(int id)
        {
            // ACT
            var result = this.equityController.Get(id);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
            this.equityManager.Verify(x => x.GetById(It.Is<int>(i => i == id)), Times.Once);
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
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>())).Throws(new ArgumentOutOfRangeException());
            
            // ACT
            var result = this.equityController.Get(id);

            // ASSERT
            Assert.IsType<BadRequestResult>(result);
            this.equityManager.Verify(x => x.GetById(It.Is<int>(i => i == id)), Times.Once);
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
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Equity { ID = id });
            
            // ACT
            var result = this.equityController.Get(id) as OkObjectResult;

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Equity>(result.Value);
            Assert.Equal(id, (result.Value as Equity).ID);
            this.equityManager.Verify(x => x.GetById(It.Is<int>(i => i == id)), Times.Once);
        }

        /// <summary>
        /// Created Response for Post.
        /// </summary>
        [Fact]
        public void Post_ReturnsCreatedResponse()
        {
            // ARRANGE
            this.equityManager.Setup(x => x.Insert(It.IsAny<Equity>())).Returns(1);
            var equity = new Equity { Name = "Sensex", Amount = 30000 };

            // ACT
            var createdResponse = this.equityController.Post(equity);
            
            // ASSERT
            Assert.IsType<CreatedAtActionResult>(createdResponse);
            this.equityManager.Verify(x => x.Insert(It.IsAny<Equity>()), Times.Once);
        }

        /// <summary>
        /// Validate Items Created by Post.
        /// </summary>
        [Fact]
        public void Post_ReturnedResponseHasCreatedItem()
        {
            // ARRANGE
            this.equityManager.Setup(x => x.Insert(It.IsAny<Equity>())).Returns(1);
            var equity = new Equity { Name = "Jitesh", Amount = 30000 };

            // ACT
            var createdResponse = this.equityController.Post(equity) as CreatedAtActionResult;
            var item = createdResponse.Value as Equity;

            // ASSERT
            Assert.IsType<Equity>(item);
            Assert.Equal(1, equity.ID);
            this.equityManager.Verify(x => x.Insert(It.IsAny<Equity>()), Times.Once);
        }

        /// <summary>
        /// Not Found for Put
        /// </summary>
        [Fact]
        public void Put_ReturnsNotFoundResult()
        {
            // ARRANGE
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>()));
            var equity = new Equity { Name = "Jitesh", Amount = 30000, ID = 1 };

            // ACT
            var result = this.equityController.Put(equity);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
            this.equityManager.Verify(x => x.GetById(It.Is<int>(id => id == 1)), Times.Once);
            this.equityManager.Verify(x => x.Update(It.IsAny<Equity>()), Times.Never);
        }

        /// <summary>
        /// Bad Request for Put
        /// </summary>
        [Fact]
        public void Put_ReturnsBadRequestResult()
        {
            // ARRANGE
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>())).Throws(new ArgumentOutOfRangeException());
            var equity = new Equity { Name = "Jitesh", Amount = 30000, ID = 1 };

            // ACT
            var result = this.equityController.Put(equity);

            // ASSERT
            Assert.IsType<BadRequestResult>(result);
            this.equityManager.Verify(x => x.GetById(It.Is<int>(id => id == 1)), Times.Once);
            this.equityManager.Verify(x => x.Update(It.IsAny<Equity>()), Times.Never);
        }

        /// <summary>
        /// Ok Result for Put
        /// </summary>
        [Fact]
        public void Put_ReturnsOkResult()
        {
            // ARRANGE
            var equity = new Equity { Name = "Jitesh", Amount = 30000, ID = 1 };
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>())).Returns(equity);
            this.equityManager.Setup(x => x.Update(It.IsAny<Equity>()));

            // ACT
            var result = this.equityController.Put(equity);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
            this.equityManager.Verify(x => x.GetById(It.Is<int>(id => id == 1)), Times.Once);
            this.equityManager.Verify(x => x.Update(It.IsAny<Equity>()), Times.Once);
        }

        /// <summary>
        /// Items updated for Put
        /// </summary>
        [Fact]
        public void Put_ReturnsUpdatedItems()
        {
            // ARRANGE
            var equity = new Equity { Name = "Jitesh", Amount = 30000, ID = 1 };
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>())).Returns(equity);
            this.equityManager.Setup(x => x.Update(It.IsAny<Equity>()));

            // ACT
            var result = this.equityController.Put(equity) as OkObjectResult;
            var updatedItem = result.Value as Equity;

            // ASSERT
            Assert.IsType<Equity>(updatedItem);
            Assert.Equal(equity.ID, updatedItem.ID);
            Assert.Equal(equity.Name, updatedItem.Name);
            Assert.Equal(equity.Amount, updatedItem.Amount);
            this.equityManager.Verify(x => x.GetById(It.Is<int>(id => id == 1)), Times.Once);
            this.equityManager.Verify(x => x.Update(It.IsAny<Equity>()), Times.Once);
        }

        /// <summary>
        /// Bad Request for Delete.
        /// </summary>
        [Fact]
        public void Delete_ReturnsBadRequestResult()
        {
            // ARRANGE
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>())).Throws(new ArgumentOutOfRangeException());

            // ACT
            var response = this.equityController.Delete(1);

            // ASSERT
            Assert.IsType<BadRequestResult>(response);
            this.equityManager.Verify(x => x.Delete(It.Is<int>(id => id == 1)), Times.Never);
        }

        // Not Found for Delete.
        [Fact]
        public void Delete_ReturnsNotFoundResponse()
        {
            // ARRANGE
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>()));

            // ACT
            var response = this.equityController.Delete(1);

            // ASSERT
            Assert.IsType<NotFoundResult>(response);
            this.equityManager.Verify(x => x.Delete(It.Is<int>(id => id == 1)), Times.Never);
        }

        /// <summary>
        /// No content for Delete
        /// </summary>
        [Fact]
        public void Delete_ReturnsNoContentResult()
        {
            // ARRANGE
            this.equityManager.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Equity { ID = 1 });
            this.equityManager.Setup(x => x.Delete(It.IsAny<int>()));

            // ACT
            var response = this.equityController.Delete(1);

            // ASSERT
            Assert.IsType<NoContentResult>(response);
            this.equityManager.Verify(x => x.Delete(It.Is<int>(id => id == 1)), Times.Once);
        }
    }
}

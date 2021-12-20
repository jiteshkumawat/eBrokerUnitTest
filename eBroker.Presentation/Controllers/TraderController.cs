using eBroker.Business;
using eBroker.Core;
using eBroker.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eBroker.Presentation.Controllers
{
    /// <summary>
    /// Trader Controller Class
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TraderController : ControllerBase
    {
        /// <summary>
        /// Trader Manager
        /// </summary>
        private readonly ITraderManager traderManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraderController"/> class.
        /// </summary>
        /// <param name="manager">Trader Manager</param>
        public TraderController(ITraderManager manager)
        {
            this.traderManager = manager;
        }

        /// <summary>
        /// Get All the Traders
        /// </summary>
        /// <returns>Trader Details</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var result = this.traderManager.GetAll();
            return Ok(result);
        }

        /// <summary>
        /// Get Trader by Id
        /// </summary>
        /// <param name="id">Trader Id</param>
        /// <returns>Trader Details</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = this.traderManager.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Post Trader Data
        /// </summary>
        /// <param name="equity">Trader Details</param>
        /// <returns>Inserted Trader Record</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Trader trader)
        {
            var id = this.traderManager.Insert(trader);
            trader.ID = id;
            return CreatedAtAction("Get", new { id = id }, trader);
        }

        /// <summary>
        /// Update Trader Data
        /// </summary>
        /// <param name="equity">Trader Details</param>
        /// <returns>Updated Trader Record</returns>
        [HttpPut]
        public IActionResult Put([FromBody] Trader trader)
        {
            try
            {
                var result = this.traderManager.GetById(trader.ID);
                if (result == null)
                {
                    return NotFound();
                }

                this.traderManager.Update(trader);
                return Ok(trader);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete Trader Records
        /// </summary>
        /// <param name="id">Trader Identifier</param>
        /// <returns>Status of Delete operation</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = this.traderManager.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }

                this.traderManager.Delete(id);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Buy Equities
        /// </summary>
        /// <param name="traderId">Trader Id</param>
        /// <param name="equityId">Equity Id</param>
        /// <param name="quantity">Quantity to purchase</param>
        /// <param name="time">Time of operation</param>
        /// <returns>Status of Operation</returns>
        [HttpPost]
        [Route("buy")]
        public IActionResult BuyEquity(int traderId, int equityId, int quantity, DateTime time)
        {
            try
            {
                bool status = this.traderManager.BuyEquity(traderId, equityId, quantity, time);
                if (!status)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(ex.EntityName);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Sell Equities
        /// </summary>
        /// <param name="traderId">Trader Id</param>
        /// <param name="equityId">Equity Id</param>
        /// <param name="quantity">Quantity to Sell</param>
        /// <param name="time">Time of operation</param>
        /// <returns>Status of Operation</returns>
        [HttpPost]
        [Route("sell")]
        public IActionResult SellEquity(int traderId, int equityId, int quantity, DateTime time)
        {
            try
            {
                bool status = this.traderManager.SellEquity(traderId, equityId, quantity, time);
                if (!status)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(ex.EntityName);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Add Trader Funds
        /// </summary>
        /// <param name="traderId">Traer Id</param>
        /// <param name="amount">Amount to increase</param>
        /// <returns>Status of Operation</returns>
        [HttpPost]
        [Route("addfunds")]
        public IActionResult AddFunds(int traderId, double amount)
        {
            try
            {
                bool status = this.traderManager.AddFunds(traderId, amount);
                if (!status)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(ex.EntityName);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }
    }
}

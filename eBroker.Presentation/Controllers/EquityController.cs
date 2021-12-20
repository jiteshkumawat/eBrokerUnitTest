using eBroker.Business;
using eBroker.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eBroker.Presentation.Controllers
{
    /// <summary>
    /// Equity Controller Class
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EquityController : ControllerBase
    {
        /// <summary>
        /// Equity Manager
        /// </summary>
        private readonly IManager<Equity> manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquityController"/> class.
        /// </summary>
        /// <param name="manager">Equity Manager</param>
        public EquityController(IManager<Equity> manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Get All the Equities
        /// </summary>
        /// <returns>Equity Details</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var equities = this.manager.GetAll();
            return Ok(equities);
        }

        /// <summary>
        /// Get Equity by Id
        /// </summary>
        /// <param name="id">Equity Id</param>
        /// <returns>Equity Details</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Equity result = this.manager.GetById(id);
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
        /// Post Equity Data
        /// </summary>
        /// <param name="equity">Equity Details</param>
        /// <returns>Inserted Equity Record</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Equity equity)
        {
            var id = this.manager.Insert(equity);
            equity.ID = id;
            return CreatedAtAction("Get", new { id = id }, equity);
        }

        /// <summary>
        /// Update Equity Data
        /// </summary>
        /// <param name="equity">Equity Details</param>
        /// <returns>Updated Equity Record</returns>
        [HttpPut]
        public IActionResult Put([FromBody] Equity equity)
        {
            try
            {
                Equity result = this.manager.GetById(equity.ID);
                if (result == null)
                {
                    return NotFound();
                }

                this.manager.Update(equity);
                return Ok(equity);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete Equity Records
        /// </summary>
        /// <param name="id">Equity Identifier</param>
        /// <returns>Status of Delete operation</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Equity result = this.manager.GetById(id);
                if (result == null)
                {
                    return NotFound();
                }

                this.manager.Delete(id);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
        }
    }
}

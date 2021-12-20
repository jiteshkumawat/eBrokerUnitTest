using eBroker.Model;
using System;

namespace eBroker.Core
{
    /// <summary>
    /// Operations Utility Proxy Contract
    /// </summary>
    public interface IOperationsUtilityProxy
    {
        /// <summary>
        /// Add Equities
        /// </summary>
        /// <param name="trader">Trader Instance</param>
        /// <param name="equity">Equity Instance</param>
        /// <param name="quantity">Quantity of Equity</param>
        /// <returns>Add Equity Status</returns>
        bool AddEquities(Trader trader, Equity equity, int quantity);

        /// <summary>
        /// Reduce Equities
        /// </summary>
        /// <param name="trader">Trader Instance</param>
        /// <param name="equityId">Equity Instance</param>
        /// <param name="quantity">Quantity of Equity</param>
        /// <returns>Reduce Equity Status</returns>
        bool ReduceEquities(Trader trader, int equityId, int quantity);

        /// <summary>
        /// Increase Funds
        /// </summary>
        /// <param name="trader">Trader Details</param>
        /// <param name="amount">Amount to increase</param>
        /// <returns>Status of Increase Funds</returns>
        bool IncreaseFunds(Trader trader, double amount);

        /// <summary>
        /// Identify if time is operating hours
        /// </summary>
        /// <param name="time">Time to operate</param>
        /// <returns>Status of Is Operating Hours</returns>
        bool IsOperatingHours(DateTime time);
    }
}

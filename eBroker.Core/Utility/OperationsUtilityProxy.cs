using eBroker.Model;
using System;

namespace eBroker.Core
{
    /// <summary>
    /// Proxy around Operation Utility
    /// </summary>
    public class OperationsUtilityProxy: IOperationsUtilityProxy
    {
        /// <summary>
        /// Add Equities
        /// </summary>
        /// <param name="trader">Trader Instance</param>
        /// <param name="equity">Equity Instance</param>
        /// <param name="quantity">Quantity of Equity</param>
        /// <returns>Add Equity Status</returns>
        public bool AddEquities(Trader trader, Equity equity, int quantity)
        {
            return OperationsUtility.AddEquities(trader, equity, quantity);
        }

        /// <summary>
        /// Reduce Equities
        /// </summary>
        /// <param name="trader">Trader Instance</param>
        /// <param name="equityId">Equity Instance</param>
        /// <param name="quantity">Quantity of Equity</param>
        /// <returns>Reduce Equity Status</returns>
        public bool ReduceEquities(Trader trader, int equityId, int quantity)
        {
            return OperationsUtility.ReduceEquities(trader, equityId, quantity);
        }

        /// <summary>
        /// Increase Funds
        /// </summary>
        /// <param name="trader">Trader Details</param>
        /// <param name="amount">Amount to increase</param>
        /// <returns>Status of Increase Funds</returns>
        public bool IncreaseFunds(Trader trader, double amount)
        {
            return OperationsUtility.IncreaseFunds(trader, amount);
        }

        /// <summary>
        /// Identify if time is operating hours
        /// </summary>
        /// <param name="time">Time to operate</param>
        /// <returns>Status of Is Operating Hours</returns>
        public bool IsOperatingHours(DateTime time)
        {
            return OperationsUtility.IsWorkingHours(time);
        }
    }
}

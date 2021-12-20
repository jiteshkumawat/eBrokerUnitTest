using eBroker.Model;
using System;
using System.Linq;

namespace eBroker.Core
{
    /// <summary>
    /// Opertion Utility
    /// </summary>
    public static class OperationsUtility
    {
        /// <summary>
        /// Add Equities
        /// </summary>
        /// <param name="trader">Trader Instance</param>
        /// <param name="equity">Equity Instance</param>
        /// <param name="quantity">Quantity of Equity</param>
        /// <returns>Add Equity Status</returns>
        public static bool AddEquities(Trader trader, Equity equity, int quantity)
        {
            var totalAmount = equity.Amount * quantity;
            if (trader.Funds < totalAmount)
            {
                return false;
            }

            trader.Funds -= totalAmount;
            var traderEquity = trader.TraderEquities.FirstOrDefault(x => x.EquityId == equity.ID);
            if (traderEquity != null)
            {
                traderEquity.Quantity += quantity;
            }
            else
            {
                var newTraderEquity = new TraderEquity { Equity = equity, EquityId = equity.ID, Quantity = quantity, Trader = trader, TraderId = trader.ID };
                trader.TraderEquities.Add(newTraderEquity);
            }

            return true;
        }

        /// <summary>
        /// Reduce Equities
        /// </summary>
        /// <param name="trader">Trader Instance</param>
        /// <param name="equityId">Equity Instance</param>
        /// <param name="quantity">Quantity of Equity</param>
        /// <returns>Reduce Equity Status</returns>
        public static bool ReduceEquities(Trader trader, int equityId, int quantity)
        {
            // Get Equity
            var traderEquity = trader.TraderEquities.FirstOrDefault(x => x.EquityId == equityId);

            if (traderEquity == null || traderEquity.Quantity < quantity)
            {
                return false;
            }

            var totalAmount = traderEquity.Equity.Amount * quantity;
            var brokage = totalAmount * 0.0005;
            if (brokage < 20)
            {
                brokage = 20;
            }

            if (brokage > totalAmount + trader.Funds)
            {
                brokage = totalAmount + trader.Funds;
            }

            totalAmount -= brokage;

            trader.Funds += totalAmount;
            traderEquity.Quantity -= quantity;
            return true;
        }

        /// <summary>
        /// Increase Funds
        /// </summary>
        /// <param name="trader">Trader Details</param>
        /// <param name="amount">Amount to increase</param>
        /// <returns>Status of Increase Funds</returns>
        public static bool IncreaseFunds(Trader trader, double amount)
        {
            if (amount > 100000)
            {
                amount -= (amount * 0.0005);
            }

            trader.Funds += amount;
            return true;
        }

        /// <summary>
        /// Identify if time is operating hours
        /// </summary>
        /// <param name="time">Time to operate</param>
        /// <returns>Status of Is Operating Hours</returns>
        public static bool IsWorkingHours(DateTime time)
        {
            if (time.Hour < 9 || time.Hour >= 15)
            {
                throw new ArgumentOutOfRangeException("time", "Time of Operation should be between 9:00 AM and 3:00 PM");
            }

            return true;
        }
    }
}

using eBroker.Model;
using System;

namespace eBroker.Business
{
    /// <summary>
    /// Contract for Trader Manager
    /// </summary>
    public interface ITraderManager: IManager<Trader>
    {
        /// <summary>
        /// Buy Equities
        /// </summary>
        /// <param name="traderId">Trader Identifier</param>
        /// <param name="equityId">Equity Identifier</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="time">Time of Purchase</param>
        /// <returns>True if Buy Equity is successful</returns>
        bool BuyEquity(int traderId, int equityId, int quantity, DateTime time);

        /// <summary>
        /// Sell Equity
        /// </summary>
        /// <param name="traderId">Trader Identifier</param>
        /// <param name="equityId">Equity Identifier</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="time">Time of Purchase</param>
        /// <returns>True if Sell Equity is successful</returns>
        bool SellEquity(int traderId, int equityId, int quantity, DateTime time);

        /// <summary>
        /// Add Funds
        /// </summary>
        /// <param name="traderId">Trader Identifier</param>
        /// <param name="amount">Amount to add</param>
        /// <returns>True if add amount is successful</returns>
        bool AddFunds(int traderId, double amount);
    }
}

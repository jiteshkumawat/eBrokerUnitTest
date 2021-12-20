using eBroker.Core;
using eBroker.DAL;
using eBroker.Model;
using System;

namespace eBroker.Business
{
    /// <summary>
    /// Trader Manager Class
    /// </summary>
    public class TraderManager : ManagerBase<Trader>, ITraderManager
    {
        /// <summary>
        /// Equity Repository
        /// </summary>
        private IRepository<Equity> equityRepository;

        /// <summary>
        /// Utility Services
        /// </summary>
        private IOperationsUtilityProxy operationsUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraderManager"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public TraderManager(IRepository<Trader> repository, IRepository<Equity> equityRepository, IOperationsUtilityProxy operationsUtility)
            : base(repository)
        {
            this.equityRepository = equityRepository;
            this.operationsUtility = operationsUtility;
        }

        /// <summary>
        /// Buy Equities
        /// </summary>
        /// <param name="traderId">Trader Identifier</param>
        /// <param name="equityId">Equity Identifier</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="time">Time of Purchase</param>
        /// <returns>True if Buy Equity is successful</returns>
        public bool BuyEquity(int traderId, int equityId, int quantity, DateTime time)
        {
            bool status = this.operationsUtility.IsOperatingHours(time);

            if (status)
            {
                // Get Trader
                var trader = this.GetModel<Trader>(this.repository, traderId);

                // Get Equity
                var equity = this.GetModel<Equity>(this.equityRepository, equityId);

                // Update Equities
                status = this.operationsUtility.AddEquities(trader, equity, quantity);

                // Update Records
                if (status)
                {
                    this.repository.Update(trader);
                }
            }

            return status;
        }

        /// <summary>
        /// Sell Equity
        /// </summary>
        /// <param name="traderId">Trader Identifier</param>
        /// <param name="equityId">Equity Identifier</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="time">Time of Purchase</param>
        /// <returns>True if Sell Equity is successful</returns>
        public bool SellEquity(int traderId, int equityId, int quantity, DateTime time)
        {
            bool status = this.operationsUtility.IsOperatingHours(time);

            if (status)
            {
                // Get Trader
                var trader = this.GetModel<Trader>(this.repository, traderId);

                // Update Equities
                status = this.operationsUtility.ReduceEquities(trader, equityId, quantity);

                // Update Records
                if (status)
                {
                    this.repository.Update(trader);
                }
            }

            return status;
        }

        /// <summary>
        /// Add Funds
        /// </summary>
        /// <param name="traderId">Trader Identifier</param>
        /// <param name="amount">Amount to add</param>
        /// <returns>True if add amount is successful</returns>
        public bool AddFunds(int traderId, double amount)
        {
            // Get Trader
            var trader = this.GetModel<Trader>(this.repository, traderId);

            // Update Funds
            var status = this.operationsUtility.IncreaseFunds(trader, amount);

            // Update Traders
            if (status)
            {
                this.repository.Update(trader);
            }
            return status;
        }

        /// <summary>
        /// Get Model from repository
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <param name="repository">Repository Type</param>
        /// <param name="id">Model Identifier</param>
        /// <returns>Model Details</returns>
        private T GetModel<T>(IRepository<T> repository, int id)
        {
            var model = repository.Get(id);
            if (model == null)
            {
                throw new RecordNotFoundException(typeof(T).Name);
            }

            return model;
        }
    }
}

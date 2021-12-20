namespace eBroker.Model
{
    /// <summary>
    /// Equity With Traders
    /// </summary>
    public class TraderEquity
    {
        /// <summary>
        /// Trader Equity Identifier
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Trader Identifier
        /// </summary>
        public int TraderId { get; set; }

        /// <summary>
        /// Trader Instance
        /// </summary>
        public virtual Trader Trader { get; set; }

        /// <summary>
        /// Equity Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Equity Identifier
        /// </summary>
        public int EquityId { get; set; }

        /// <summary>
        /// Equity Instance
        /// </summary>
        public virtual Equity Equity { get; set; }
    }
}

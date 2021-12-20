using System.Collections.Generic;

namespace eBroker.Model
{
    /// <summary>
    /// Trader Model
    /// </summary>
    public class Trader: BaseModel
    {
        /// <summary>
        /// Trader Funds
        /// </summary>
        public double Funds { get; set; }

        /// <summary>
        /// Equities with Traders
        /// </summary>
        public ICollection<TraderEquity> TraderEquities { get; set; }
    }
}

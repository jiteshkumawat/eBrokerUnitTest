using System.Collections;
using System.Collections.Generic;

namespace eBroker.Test
{
    /// <summary>
    /// Equity Data Source
    /// </summary>
    public class EquityDataSource : IEnumerable<object[]>
    {
        /// <summary>
        /// Equity Data
        /// </summary>
        private readonly List<object[]> data = new List<object[]>
        {
            new object[] { "Sensex", 57788.03, 1 },
            new object[] { "Nifty", 17221.40, 2 }
        };

        /// <summary>
        /// Get Enumerator Implementation
        /// </summary>
        /// <returns>Equity Data</returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns>Equity Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

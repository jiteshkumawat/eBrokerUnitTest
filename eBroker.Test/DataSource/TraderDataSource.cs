using System.Collections;
using System.Collections.Generic;

namespace eBroker.Test
{
    /// <summary>
    /// Trader Data Source
    /// </summary>
    public class TraderDataSource : IEnumerable<object[]>
    {
        /// <summary>
        /// Trader Data
        /// </summary>
        private readonly List<object[]> data = new List<object[]>
        {
            new object[] { "Jitesh", 30000, 1 },
            new object[] { "Neha", 60000, 2 }
        };

        /// <summary>
        /// Get Enumerator Implementation
        /// </summary>
        /// <returns>Trader Data</returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns>Trader Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

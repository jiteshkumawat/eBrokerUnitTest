using eBroker.DAL;
using eBroker.Model;
using System;
using System.Collections.Generic;

namespace eBroker.Business
{
    /// <summary>
    /// Equity Manager Class
    /// </summary>
    public class EquityManager : ManagerBase<Equity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquityManager"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public EquityManager(IRepository<Equity> repository)
            :base(repository)
        {
        }
    }
}

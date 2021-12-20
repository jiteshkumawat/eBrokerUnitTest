using System;
namespace eBroker.Core
{
    /// <summary>
    /// Exception class for Data Not Found
    /// </summary>
    public class RecordNotFoundException : Exception
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Non Parameterized Constructor
        /// </summary>
        public RecordNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Record Name
        /// </summary>
        /// <param name="entity"></param>
        public RecordNotFoundException(string entity)
            : base("No record found for " + entity)
        {
            this.EntityName = entity;
        }
    }
}

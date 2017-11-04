using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Webdriver.CascadeCommands
{

    /// <summary>
    /// Hold success or fault status with any information about a command execution.
    /// </summary>
    public class ExecutionRegistry
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The Id to indetify the registry.</param>
        /// <param name="success">True or false status.</param>
        /// <param name="message">Any given text to inform a execution registry.</param>
        public ExecutionRegistry(int id, bool success, string message)
        {
            this.Id = id;
            this.Success = success;
            this.Message = message;
        }

        /// <summary>
        /// The Id to indetify the registry
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// True or false status.
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// Any given text to inform a execution registry.
        /// </summary>
        public string Message { get; protected set; }
    }
}

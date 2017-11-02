using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Webdriver.CascadeCommands
{
    public class ExecutionRegistry
    {
        public ExecutionRegistry(int id, bool sucess, string message)
        {
            this.Id = id;
            this.Success = sucess;
            this.Message = message;
        }

        public int Id { get; protected set; }

        public bool Success { get; protected set; }

        public string Message { get; protected set; }
    }
}

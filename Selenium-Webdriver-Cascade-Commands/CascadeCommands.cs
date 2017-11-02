using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Selenium.Webdriver.CascadeCommands
{


    public class CascadeCommands
    {
        protected IWebDriver driver = null;
        protected IList<ExecutionRegistry> listExecutionRegistry;
        protected IJavaScriptExecutor jsExecutor = null;

        public CascadeCommands(IWebDriver driver)
        {
            this.driver = driver;
            this.jsExecutor = driver as IJavaScriptExecutor;
            this.listExecutionRegistry = new List<ExecutionRegistry>();
        }


        /// <summary>
        /// Clear the execution registry list.
        /// </summary>
        public void ClearExecutionRegistry()
        {
            this.listExecutionRegistry.Clear();
        }


        /// <summary>
        /// Get an Array of the all execution registry.
        /// </summary>
        /// <returns></returns>
        public ExecutionRegistry[] GetExecutionRegistry()
        {
            return this.listExecutionRegistry.ToArray<ExecutionRegistry>();
        }


        /// <summary>
        /// Get an Array of the all errors execution registry.
        /// </summary>
        /// <returns></returns>
        public ExecutionRegistry[] GetExecutionRegistryWithErrors()
        {
            return this.listExecutionRegistry.Where(o => o.Success == false)
                                             .ToArray<ExecutionRegistry>();
        }


        /// <summary>
        /// Get an Array of the all errors execution registry.
        /// </summary>
        /// <returns></returns>
        public ExecutionRegistry[] GetExecutionRegistryWithSuccess()
        {
            return this.listExecutionRegistry.Where(o => o.Success)
                                             .ToArray<ExecutionRegistry>();
        }


        /// <summary>
        /// Indicate even whith any error the execution flow will continues.
        /// </summary>
        public bool ExecuteWithError { get; set; }


        /// <summary>
        /// Holds any object for the just finished execution.
        /// </summary>
        public object CurrentElement { get; protected set; }


        /// <summary>
        /// Indicates if it has error.
        /// </summary>
        public bool HasError { get; protected set; }


        /// <summary>
        /// Set the Current element explicity.
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public CascadeCommands SetCurrent(IWebElement webElement)
        {
            if (CancelExecution())
                return this;


            this.CurrentElement = webElement;
            AddRegistrySuccess("Current Element has setted successfully out side of the scope.");

            return this;
        }


        /// <summary>
        /// Clears the content of the Current Element.
        /// </summary>
        /// <returns></returns>
        public CascadeCommands Clear()
        {
            if (CancelExecution())
                return this;


            if (NoCurrent())
            {
                AddRegistryFault("There is no Current Element to Clear! Check the previous operation.");
                return this;
            }

            try
            {
                IWebElement webElement = (IWebElement)this.CurrentElement;
                webElement.Clear();

                AddRegistrySuccess("The Clear operation was executed successfully.");
            }
            catch (Exception ex)
            {
                AddRegistryFault("Clear execution has thrown some error : " + ex.Message);
            }

            return this;
        }


        /// <summary>
        /// Find the fist IWebElement using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElement(By by, string labelCommand)
        {
            if (CancelExecution())
                return this;

            try
            {
                IWebElement webElement = driver.FindElement(by);
                this.CurrentElement = webElement;

                AddRegistrySuccess("Find Element was executed sucessfully.", labelCommand);
            }
            catch (Exception ex)
            {
                AddRegistryFault("Find Element execution has thrown some error  : " + ex.Message, labelCommand);
            }

            return this;
        }


        /// <summary>
        /// Find the fist IWebElement using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns></returns>
        public CascadeCommands FindElement(By by)
        {
            return FindElement(by, string.Empty);
        }


        /// <summary>
        /// Clicks the element.
        /// </summary>
        /// <returns></returns>
        public CascadeCommands Click()
        {
            if (CancelExecution())
                return this;


            if (NoCurrent())
            {
                AddRegistryFault("There is no Current Element to Click! Check the previous operation.");
                return this;
            }


            try
            {
                IWebElement webElement = (IWebElement)this.CurrentElement;
                webElement.Click();

                AddRegistrySuccess("Click execution was executed sucessfully.");
            }
            catch (Exception ex)
            {
                AddRegistryFault("Click Element execution has thrown some error : " + ex.Message);
            }

            return this;
        }


        /// <summary>
        /// Force the current exectution thread to sleeps.
        /// </summary>
        /// <param name="milisecondsTimeout">Miliseconds to sleep.</param>
        /// <returns></returns>
        public CascadeCommands Sleep(int milisecondsTimeout)
        {
            if (CancelExecution())
                return this;

            Thread.Sleep(milisecondsTimeout);
            AddRegistrySuccess("Slept for " + milisecondsTimeout);

            return this;
        }


        /// <summary>
        /// Simulates typing text into the current element.
        /// </summary>
        /// <param name="txt">The text to type into the element.</param>
        /// <returns></returns>
        public CascadeCommands SendKeys(string txt)
        {
            if (CancelExecution())
                return this;

            if (NoCurrent())
            {
                AddRegistryFault("There is no Current Element to SendKeys! Check the previous operation.");
                return this;
            }


            try
            {
                IWebElement webElement = (IWebElement)this.CurrentElement;
                webElement.SendKeys(txt);

                AddRegistrySuccess("SendKeys execution was executed sucessfully.");
            }
            catch (Exception ex)
            {
                AddRegistryFault("SendKeys Element execution has thrown some error : " + ex.Message);
            }



            return this;
        }


        /// <summary>
        /// Submits the current element to the web server.
        /// </summary>
        /// <returns></returns>
        public CascadeCommands Submit()
        {
            if (CancelExecution())
                return this;

            if (NoCurrent())
            {
                AddRegistryFault("There is no Current Element to Submit! Check the previous operation.");
                return this;
            }

            try
            {
                IWebElement webElement = (IWebElement)this.CurrentElement;
                webElement.Submit();

                AddRegistrySuccess("Submit execution was executed sucessfully.");
            }
            catch (Exception ex)
            {
                AddRegistryFault("Submit Element execution has thrown some error : " + ex.Message);
            }



            return this;
        }


        /// <summary>
        /// Executes the JavaScript in the context of the currently selected frame or window on current element.
        /// </summary>
        /// <param name="script">The JavaScript to execute.</param>
        /// <returns></returns>
        public CascadeCommands ExecuteScriptOnCurrentElement(string script)
        {
            if (CancelExecution())
                return this;

            if (NoCurrent())
            {
                AddRegistryFault("There is no Current Element to ExecuteScript! Check the previous operation.");
                return this;
            }


            try
            {
                IWebElement webElement = (IWebElement)this.CurrentElement;
                this.jsExecutor.ExecuteScript(script, webElement);

                AddRegistrySuccess("JavaScript execution was executed successfully.");
            }
            catch (Exception ex)
            {
                AddRegistryFault("JavaScript execution has thrown some error : " + ex.Message);
            }


            return this;
        }


        /// <summary>
        /// Executes the JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript to execute.</param>
        /// <returns></returns>
        public CascadeCommands ExecuteScript(string script)
        {
            if (CancelExecution())
                return this;

            try
            {
                this.jsExecutor.ExecuteScript(script);

                AddRegistrySuccess("JavaScript execution was executed successfully.");
            }
            catch (Exception ex)
            {
                AddRegistryFault("JavaScript execution has thrown some error : " + ex.Message);
            }


            return this;
        }


        /// <summary>
        /// Select all opitons by the text displayed.
        /// </summary>
        /// <param name="text">The text of the option to be selected. If an exact match is not found, this method will perform a substring method.</param>
        /// <returns></returns>
        public CascadeCommands SelectByText(string text)
        {

            if (CancelExecution())
                return this;

            if (NoCurrent())
            {
                AddRegistryFault("There is no Current Element to SelectByText! Check the previous operation.");
                return this;
            }


            try
            {
                IWebElement webElement = (IWebElement)this.CurrentElement;
                var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(webElement);
                selectElement.SelectByText(text);

                AddRegistrySuccess("SelectByText was executed successfully!");
            }
            catch (Exception ex)
            {
                AddRegistryFault("SelectByText execution has thrown some error : " + ex.Message);
            }



            return this;
        }


        /// <summary>
        /// Select an option by the value.
        /// </summary>
        /// <param name="value">THe value of the option to be selected.</param>
        /// <returns></returns>
        public CascadeCommands SelectByValue(string value)
        {
            if (CancelExecution())
                return this;

            if (NoCurrent())
            {
                AddRegistryFault("There is no Current Element to SelectByValue! Check the previous operation.");
                return this;
            }
            try
            {
                IWebElement webElement = (IWebElement)this.CurrentElement;
                var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(webElement);
                selectElement.SelectByValue(value);

                AddRegistrySuccess("SelectByValue was executed successfully.");
            }
            catch (Exception ex)
            {
                AddRegistryFault("SelectByValue execution has thrown some error : " + ex.Message);
            }

            return this;
        }


        /// <summary>
        /// Checks if there is no Current Element
        /// </summary>
        /// <returns></returns>
        protected bool NoCurrent()
        {
            if (this.CurrentElement == null)
                return true;

            return false;
        }


        /// <summary>
        /// Checks if is to cancel the execution flow.
        /// </summary>
        /// <returns></returns>
        protected bool CancelExecution()
        {
            if (!this.ExecuteWithError && this.HasError)
                return true;

            return false;
        }


        /// <summary>
        /// Add one message to the Registry Execution list.
        /// </summary>
        /// <param name="sucess">Indicates sucess or fault.</param>
        /// <param name="message">The message to insert into.</param>
        protected void AddRegistry(bool sucess, string message)
        {
            int nextId = this.listExecutionRegistry.Count() + 1;
            this.listExecutionRegistry.Add(new ExecutionRegistry(nextId, sucess, message));
        }


        /// <summary>
        /// Add one Fault message to the Registry Execution list.
        /// </summary>
        /// <param name="message">The message to insert into.</param>
        protected void AddRegistryFault(string message)
        {
            this.HasError = true;
            this.CurrentElement = null;

            int nextId = this.listExecutionRegistry.Count() + 1;
            this.listExecutionRegistry.Add(new ExecutionRegistry(nextId, false, message));
        }


        /// <summary>
        /// Add one Fault message to the Registry Execution list.
        /// </summary>
        /// <param name="message">The message to insert into.</param>
        /// <param name="labelCommand">The any text related to the command.</param>
        protected void AddRegistryFault(string message, string labelCommand)
        {
            if (!string.IsNullOrEmpty(labelCommand))
            {
                message = message + " - " + labelCommand;
            }

            AddRegistryFault(message);
        }


        /// <summary>
        /// Add one Sucess message to the Registry Execution list.
        /// </summary>
        /// <param name="message"></param>
        protected void AddRegistrySuccess(string message)
        {
            int nextId = this.listExecutionRegistry.Count() + 1;
            this.listExecutionRegistry.Add(new ExecutionRegistry(nextId, true, message));
        }


        /// <summary>
        /// Add one Sucess message to the Registry Execution list.
        /// </summary>
        /// <param name="message">The message to insert into.</param>
        /// <param name="labelCommand">The any text related to the command.</param>
        protected void AddRegistrySuccess(string message, string labelCommand)
        {
            if (!string.IsNullOrEmpty(labelCommand))
            {
                message = message + " - " + labelCommand;
            }

            AddRegistrySuccess(message);
        }



    }





}

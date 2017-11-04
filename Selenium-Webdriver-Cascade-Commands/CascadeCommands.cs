using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Selenium.Webdriver.CascadeCommands
{

    /// <summary>
    /// Wrappers Selenium Webdriver with command methods using Cascade Design Pattern.
    /// </summary>
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
        /// Get an Array of the all success execution registry.
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


        protected CascadeCommands FindOnDriver(By by, string labelCommand, string successMessage, string faultMessage)
        {
            if (CancelExecution())
                return this;

            try
            {
                IWebElement webElement = driver.FindElement(by);
                this.CurrentElement = webElement;

                AddRegistrySuccess(successMessage, labelCommand);
            }
            catch (Exception ex)
            {
                AddRegistryFault(faultMessage + Environment.NewLine + ex.Message, labelCommand);
            }

            return this;
        }


        protected CascadeCommands FindOnDriver(By by, string successMessage, string faultMessage)
        {
            return FindOnDriver(by, string.Empty, successMessage, faultMessage);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElement(By by, string labelCommand)
        {
            return FindOnDriver(by, labelCommand, "Find Element was executed successfully.", "Find Element execution has thrown some error.");
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


        #region FindElementBy

        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="id">A valid Id.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElementById(string id, string labelCommand)
        {
            return FindOnDriver(By.Id(id), labelCommand, "Find Element by Id was executed successfully.", "Find Element by Id execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="id">A valid Id.</param>
        /// <returns></returns>
        public CascadeCommands FindElementById(string id)
        {
            return FindElementById(id, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="name">A valid Name.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByName(string name, string labelCommand)
        {
            return FindOnDriver(By.Name(name), labelCommand, "Find Element by Name was executed successfully.", "Find Element by Name execution has thrown some error.");
        }

        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="name">A valid Name.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByName(string name)
        {
            return FindElementByName(name, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="xPath">A valid XPath.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByXPath(string xPath, string labelCommand)
        {
            return FindOnDriver(By.XPath(xPath), labelCommand, "Find Element by XPath was executed successfully.", "Find Element by XPath execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="xPath">A valid XPath.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByXPath(string xPath)
        {
            return FindElementByXPath(xPath, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="tagName">A valid TagName.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByTagName(string tagName, string labelCommand)
        {
            return FindOnDriver(By.TagName(tagName), labelCommand, "Find Element by TagName was executed successfully.", "Find Element by TagName execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="tagName">A valid TagName.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByTagName(string tagName)
        {
            return FindElementByTagName(tagName, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="partialLinkText">A valid PartialLinkText.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByPartialLinkText(string partialLinkText, string labelCommand)
        {
            return FindOnDriver(By.PartialLinkText(partialLinkText), labelCommand, "Find Element by PartialLinkText was executed successfully.", "Find Element by PartialLinkText execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="partialLinkText">A valid PartialLinkText.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByPartialLinkText(string partialLinkText)
        {
            return FindElementByPartialLinkText(partialLinkText, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="linkText">A valid LinkText.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByLinkText(string linkText, string labelCommand)
        {
            return FindOnDriver(By.LinkText(linkText), labelCommand, "Find Element by LinkText was executed successfully.", "Find Element by LinkText execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="linkText">A valid LinkText.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByLinkText(string linkText)
        {
            return FindElementByLinkText(linkText, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="cssSelector">A valid  CssSelector.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByCssSelector(string cssSelector, string labelCommand)
        {
            return FindOnDriver(By.CssSelector(cssSelector), labelCommand, "Find Element by CssSelector was executed successfully.", "Find Element by CssSelector execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="cssSelector">A valid  CssSelector.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByCssSelector(string cssSelector)
        {
            return FindElementByCssSelector(cssSelector, string.Empty);
        }

        
        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="className">A valid ClassName.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByClassName(string className, string labelCommand)
        {
            return FindOnDriver(By.ClassName(className), labelCommand, "Find Element by ClassName was executed successfully.", "Find Element by ClassName execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Driver.
        /// </summary>
        /// <param name="className">A valid ClassName.</param>
        /// <returns></returns>
        public CascadeCommands FindElementByClassName(string className)
        {
            return FindElementByClassName(className, string.Empty);
        }


        #endregion



        #region OnCurrentElementFind

        protected CascadeCommands OnCurrentElementFind(By by, string commandName, string labelCommand, string successMessage, string faultMessage)
        {
            if (CancelExecution())
                return this;

            if (NoCurrent())
            {
                AddRegistryFault(string.Format("There is no Current Element to {0}! Check the previous operation.", commandName));
                return this;
            }


            try
            {
                IWebElement webElement = (IWebElement)this.CurrentElement;
                this.CurrentElement = webElement.FindElement(by);

                AddRegistrySuccess(successMessage, labelCommand);
            }
            catch (Exception ex)
            {
                AddRegistryFault(faultMessage + Environment.NewLine + ex.Message, labelCommand);
            }

            return this;
        }


        protected CascadeCommands OnCurrentElementFind(By by, string commandName, string successMessage, string faultMessage)
        {
            return OnCurrentElementFind(by, commandName, string.Empty, successMessage, faultMessage);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="id">A valid Id.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindById(string id, string labelCommand)
        {
            return OnCurrentElementFind(By.Id(id), "ById", labelCommand, "Find Element on Current by Id was executed successfully.", "Find Element on Current by Id execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="id">A valid Id.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindById(string id)
        {
            return OnCurrentElementFindById(id, string.Empty);
        }
        

        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="name">A valid Name.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByName(string name, string labelCommand)
        {
            return OnCurrentElementFind(By.Name(name), "ByName", labelCommand, "Find Element on Current by Name was executed successfully.", "Find Element on Current by Name execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="name">A valid Name.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByName(string name)
        {
            return OnCurrentElementFindByName(name, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="xPath">A valid XPath.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByXPath(string xPath, string labelCommand)
        {
            return OnCurrentElementFind(By.XPath(xPath), "XPath", labelCommand, "Find Element on Current by XPath was executed successfully.", "Find Element on Current by XPath execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="xPath">A valid XPath.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByXPath(string xPath)
        {
            return OnCurrentElementFindByXPath(xPath, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="tagName">A valid TagName.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByTagName(string tagName, string labelCommand)
        {
            return OnCurrentElementFind(By.TagName(tagName), "TagName", labelCommand, "Find Element on Current by TagName was executed successfully.", "Find Element on Current by TagName execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="tagName">A valid TagName.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByTagName(string tagName)
        {
            return OnCurrentElementFindByTagName(tagName, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="partialLinkText">A valid PartialLinkText.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByPartialLinkText(string partialLinkText, string labelCommand)
        {
            return OnCurrentElementFind(By.PartialLinkText(partialLinkText), "PartialLinkText", labelCommand, "Find Element on Current by PartialLinkText was executed successfully.", "Find Element on Current by PartialLinkText execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="partialLinkText">A valid PartialLinkText.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByPartialLinkText(string partialLinkText)
        {
            return OnCurrentElementFindByPartialLinkText(partialLinkText, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="linkText">A valid LinkText.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByLinkText(string linkText, string labelCommand)
        {
            return OnCurrentElementFind(By.LinkText(linkText), "LinkText", labelCommand, "Find Element on Current by LinkText was executed successfully.", "Find Element on Current by LinkText execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="linkText">A valid LinkText.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByLinkText(string linkText)
        {
            return OnCurrentElementFindByLinkText(linkText, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="cssSelector">A valid CssSelector.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByCssSelector(string cssSelector, string labelCommand)
        {
            return OnCurrentElementFind(By.CssSelector(cssSelector), "CssSelector", labelCommand, "Find Element on Current by CssSelector was executed successfully.", "Find Element on Current by CssSelector execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="cssSelector">A valid CssSelector.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByCssSelector(string cssSelector)
        {
            return OnCurrentElementFindByCssSelector(cssSelector, string.Empty);
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="className">A valid ClassName.</param>
        /// <param name="labelCommand">Label the current command with any text.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByClassName(string className, string labelCommand)
        {
            return OnCurrentElementFind(By.ClassName(className), "ClassName",labelCommand, "Find Element by ClassName was executed successfully.", "Find Element by ClassName execution has thrown some error.");
        }


        /// <summary>
        /// Find the fist IWebElement using the given method on Current Element.
        /// </summary>
        /// <param name="className">A valid ClassName.</param>
        /// <returns></returns>
        public CascadeCommands OnCurrentElementFindByClassName(string className)
        {
            return OnCurrentElementFindByClassName(className, string.Empty);
        }

        #endregion



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

                AddRegistrySuccess("Click execution was executed successfully.");
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

                AddRegistrySuccess("SendKeys execution was executed successfully.");
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

                AddRegistrySuccess("Submit execution was executed successfully.");
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
        /// <param name="success">Indicates success or fault.</param>
        /// <param name="message">The message to insert into.</param>
        protected void AddRegistry(bool success, string message)
        {
            int nextId = this.listExecutionRegistry.Count() + 1;
            this.listExecutionRegistry.Add(new ExecutionRegistry(nextId, success, message));
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
        /// Add one success message to the Registry Execution list.
        /// </summary>
        /// <param name="message"></param>
        protected void AddRegistrySuccess(string message)
        {
            int nextId = this.listExecutionRegistry.Count() + 1;
            this.listExecutionRegistry.Add(new ExecutionRegistry(nextId, true, message));
        }


        /// <summary>
        /// Add one success message to the Registry Execution list.
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

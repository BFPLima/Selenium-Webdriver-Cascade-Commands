# Selenium-Webdriver-Cascade-Commands

### Instaling from Nuget

```pm
Install-Package Selenium-Webdriver-CascadeCommands
```



### Importing the namespace

```c#
using Selenium.Webdriver.CascadeCommands;
```


### Using the lib

#### Example 1
```c#
            var chormeOptions = new ChromeOptions();
            chormeOptions.AddArgument("--disable-gpu");
            
            var chromeDriver = new ChromeDriver(chormeOptions);
            chromeDriver.Navigate().GoToUrl("https://....");
            
            CascadeCommands commands = new CascadeCommands(chromeDriver);
           
            commands.FindElement(By.Id("input1"))
                    .SendKeys("Neo")
                    .Sleep(1000)
                    .FindElement(By.Id("input2"))
                    .SendKeys("...")
                    .Sleep(1000)
                    .FindElement(By.Id("btn1"))
                    .Click();
                    
                    
```



#### Example 2
```c#
            var chormeOptions = new ChromeOptions();
            chormeOptions.AddArgument("--disable-gpu");
            
            var chromeDriver = new ChromeDriver(chormeOptions);        
           
            IList<Person> listPerson = GetList();

            foreach (Person person in listPerson)
            {
               chromeDriver.Navigate().GoToUrl("https://....");
            
               CascadeCommands commands = new CascadeCommands(chromeDriver);

               commands.FindElement(By.Id("name"))
                       .SendKeys(person.Name)
                       .Sleep(1000)
                       .FindElement(By.Name("last"))
                       .SendKeys(person.LastName)
                       .Sleep(1000)
                       .FindElement(By.Id("address"))
                       .SendKeys(person.Address)
                       .Sleep(1000)
                       .FindElement(By.XPath("//a[text()='Advanced']"))
                       .Click()
                       .Sleep(1000)
                       .FindElement(By.Id("gender"))
                       .SelectByValue(person.Gender)
                       .Sleep(1000)
                       .FindElement(By.Name("phone"))
                       .ExecuteScriptOnCurrentElement("arguments[0].style=''")
                       .SendKeys(person.phone)
                       .Sleep(1000)
                       .FindElement(By.Id("country"))
                       .SelectByText(person.Country)
                       .Sleep(1000)
                       .ExecuteScript("document.getElementById('browseButton').removeAttribute('disabled');")
                       .FindElement(By.Id("btnSubmit"))
                       .Submit();
                       
                       
                       if(commands.HasError)
                       {
                         ExecutionRegistry[] registries =  commands.GetExecutionRegistryWithErrors();
                         //Do something ...
                       }           
            }
                    
                    
```


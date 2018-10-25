using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSupportPageObjects.NUnit.Tests.PageFactoryConfig
{
    class PageStub
    {
        [FindsBy(How = How.Id, Using = "")]
        public IWebElement IWebElement { get; set; }
    }
}

using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OpenQA.Selenium;
using SeleniumSupportPageObjects.NUnit.Tests.PageFactoryConfig;

namespace SeleniumSupportPageObjects.NUnit.Tests
{
    [TestFixture]
    public class PageFactoryTests
    {
        [Test]
        public void InitElements_GetsDriverAndPage_SetsPageIWebElementProperty()
        {
            var driver = new Mock<ISearchContext>();            
            var webElementStub = new WebElementStub("Test");
            var pageStub = new PageStub();
            driver.Setup(d => d.FindElement(It.IsAny<By>())).Returns<By>(w => webElementStub);

            PageFactory.InitElements(driver.Object, pageStub);

            Assert.AreEqual(webElementStub.Text, pageStub.IWebElement.Text);


        }
    }
}

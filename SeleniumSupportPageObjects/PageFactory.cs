using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSupportPageObjects
{
    public static class PageFactory
    {
        public static void InitElements(ISearchContext driver, object page)
        {
            if (page == null || driver == null)
                return;

            PropertyInfo[] pageProperties = page.GetType().GetProperties();

            foreach (var item in pageProperties)
            {
                var itemIsIWebElement = item.PropertyType.Equals(typeof(IWebElement));
                var itemContainsFindsByAttr = item.CustomAttributes.SingleOrDefault(a => a.AttributeType.Equals(typeof(FindsByAttribute))) != null;

                if (itemIsIWebElement && itemContainsFindsByAttr)
                {
                    var attr = item.GetCustomAttributes().OfType<FindsByAttribute>().First();
                    item.SetValue(page, initialiseElement(driver, attr));
                }
            }
        }

        private static IWebElement initialiseElement(ISearchContext driver, FindsByAttribute attribute)
        {
            IWebElement returnElement = null;
            switch (attribute.How)
            {
                case How.Id:
                    returnElement = driver.FindElement(By.Id(attribute.Using));
                    break;
                case How.Name:
                    returnElement = driver.FindElement(By.Name(attribute.Using));
                    break;
                case How.TagName:
                    returnElement = driver.FindElement(By.TagName(attribute.Using));
                    break;
                case How.ClassName:
                    returnElement = driver.FindElement(By.ClassName(attribute.Using));
                    break;
                case How.CssSelector:
                    returnElement = driver.FindElement(By.CssSelector(attribute.Using));
                    break;
                case How.LinkText:
                    returnElement = driver.FindElement(By.LinkText(attribute.Using));
                    break;
                case How.PartialLinkText:
                    returnElement = driver.FindElement(By.PartialLinkText(attribute.Using));
                    break;
                case How.XPath:
                    returnElement = driver.FindElement(By.XPath(attribute.Using));
                    break;
                case How.Custom:
                    break;
            }

            return returnElement;
        }
    }
}

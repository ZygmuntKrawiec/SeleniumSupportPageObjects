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
            PropertyInfo[] pageProperties;
            if (page != null)
                pageProperties = page.GetType().GetProperties();
            else return;

            foreach (var item in pageProperties)
            {
                if (item.GetType().Equals(typeof(IWebElement)) &&
                    item.GetCustomAttributes(true).Contains(typeof(FindsByAttribute)))
                {
                    By.
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSupportPageObjects
{
    public enum How
    {        
        Id = 0,        
        Name = 1,       
        TagName = 2,       
        ClassName = 3,       
        CssSelector = 4,        
        LinkText = 5,        
        PartialLinkText = 6,        
        XPath = 7,        
        Custom = 8
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSupportPageObjects
{
    public class FindsByAttribute : Attribute
    {
        public How How { get; set; }
        public string Using { get; set; }
    }
}

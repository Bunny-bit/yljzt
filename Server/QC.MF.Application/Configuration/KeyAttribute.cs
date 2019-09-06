﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration
{
    public class KeyAttribute : Attribute
    {
        public string Value { get; set; }
        public KeyAttribute(string value)
        {
            Value = value;
        }
    }
}

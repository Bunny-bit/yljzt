using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration
{
    public class TitleAttribute : Attribute
    {
        public string Title { get; set; }
        public TitleAttribute(string title)
        {
            Title = title;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Configuration
{
    public class TabAttribute : Attribute
    {
        public string TabName { get; set; }
        public double Order { get; set; }
        public string RequiredFeatureName { get; set; }
        public TabAttribute(string tabName, double order = 1000, string requiredFeatureName = null)
        {
            TabName = tabName;
            Order = order;
            RequiredFeatureName = requiredFeatureName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QC.MF.Web.Models.Account
{
    public class ThirdPartyModel
    {
        public string ThirdPartyName { get; set; }

        public string ThirdParty { get; set; }

        public string AuthUrl { get; set; }

        public string IconUrl { get; set; }

        public bool IsBinding { get; set; }
    }
}

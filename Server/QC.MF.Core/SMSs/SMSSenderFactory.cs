using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.SMSs
{
    public class SMSFactory
    {
        public static ISMSSenderManager CreateSMSSender(string smsType = "QC")
        {
            switch (smsType)
            {
                case "Ali":
                    return new AliSMSSenderManager();
            }
            return new QCSMSSenderManager();
        }
    }
}

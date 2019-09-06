using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QC.MF.Web.QRLogin.SignalR
{
    public class QRCodeInfo
    {
        public Guid Token { get; set; }
        public string ConnectionId { get; set; }
        public DateTime DateTime { get; set; }
        public QRCodeInfo()
        {
            Token = Guid.NewGuid();
            DateTime = DateTime.Now;
        }
        public bool IsValid()
        {
            return DateTime.Now - DateTime < TimeSpan.FromSeconds(180);
        }
    }
}

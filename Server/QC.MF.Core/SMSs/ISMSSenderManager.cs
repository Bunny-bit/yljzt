using System.Threading.Tasks;

namespace QC.MF.SMSs
{
    public interface ISMSSenderManager
    {
        Task SendVerificationCode(string phoneNumber,string code);
        Task<string> Sender(string phones, string content);
    }
}

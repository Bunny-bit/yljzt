using System.Threading.Tasks;

namespace QC.MF.SMSs
{
    public interface ISMSManager
    {
        Task SendVerificationCode(string phoneNumber);
        void ValidateVerificationCode(string phoneNumber, string verificationCode, bool verifyPass = true);
    }
}

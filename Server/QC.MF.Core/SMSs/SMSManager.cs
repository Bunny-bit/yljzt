using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.UI;
using QC.MF.VerificationCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.SMSs
{
    public class SMSManager : MFDomainServiceBase, ISMSManager
    {
        private readonly ISMSSenderManager _smsSenderManager;
        private readonly IRepository<VerificationCode> _verificationCodeRepository;
        public SMSManager(ISMSSenderManager smsSenderManager,
            IRepository<VerificationCode> verificationCodeRepository)
        {
            _smsSenderManager = smsSenderManager;
            _verificationCodeRepository = verificationCodeRepository;
        }

        public Task SendVerificationCode(string phoneNumber)
        {
            DateTime d = DateTime.Now.Date;

            int count = _verificationCodeRepository.Count(n => n.PhoneNumber == phoneNumber && n.IsVerifyPass != true && n.CreationTime > d);
            if (count >= 5)
            {
                throw new UserFriendlyException("您今天已经验证出错5次，请明天再试！");
            }


            string code = VerificationCode.GetRandomCode();

            _verificationCodeRepository.Insert(new VerificationCode()
            {
                Code = code,
                PhoneNumber = phoneNumber
            });

            
            return _smsSenderManager.SendVerificationCode(phoneNumber, code);
        }

        public void ValidateVerificationCode(string phoneNumber, string verificationCode, bool verifyPass = true)
        {
            DateTime d = DateTime.Now.AddMinutes(-10);
            var code = _verificationCodeRepository
                .GetAll()
                .OrderByDescending(n => n.CreationTime)
                .FirstOrDefault(n => n.PhoneNumber == phoneNumber
                    && n.IsVerifyPass != true
                    && n.CreationTime > d
                    && n.ErrorCount <= 5);
            if (code == null || code.Code != verificationCode)
            {
                if (code != null)
                {
                    code.ErrorCount = code.ErrorCount + 1;
                    UnitOfWorkManager.Current.SaveChanges();
                }
                throw new UserFriendlyException("验证码出错！");
            }

            if (verifyPass)
            {
                code.IsVerifyPass = true;
            }
        }
    }
}

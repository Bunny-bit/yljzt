using System;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;

namespace QC.MF.Users.Dto
{

    [AutoMap(typeof(UserLoginAttempt))]
    public class UserLoginAttemptDto
    {
        public string TenancyName { get; set; }

        public string UserNameOrEmailAddress { get; set; }

        public string ClientIpAddress { get; set; }

        public string ClientName { get; set; }

        public string BrowserInfo { get; set; }

        public string Result { get; set; }

        public string CreationTime { get; set; }
    }
}

using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QC.MF.Authorization.Users;
using QC.MF.Users;

namespace QC.MF.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Profile { get; set; }
    }
}

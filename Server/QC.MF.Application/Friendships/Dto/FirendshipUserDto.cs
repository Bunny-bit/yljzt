using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using QC.MF.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Friendships.Dto
{
    [AutoMapFrom(typeof(User))]
    public class FirendshipUserDto : EntityDto<long>
    {
        public int? TenantId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string CreationTime { get; set; }
    }
}

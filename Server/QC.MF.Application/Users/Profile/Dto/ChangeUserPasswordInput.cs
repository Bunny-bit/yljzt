using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;

namespace QC.MF.Users.Profile.Dto
{
    public class ChangeUserPasswordInput
    {
        [Required]
        [DisableAuditing]
        public long UserId { get; set; }

        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using QC.MF.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Authorization.Users;
using Abp.Runtime.Validation;

namespace QC.MF.Users.Dto
{
    //Mapped to/from User in CustomDtoMapper
    /// <summary>
    /// 编辑用户
    /// </summary>
    public class UserEditDto : IPassivable, IShouldNormalize, ICustomValidate
    {

        public UserEditDto()
        {
        }
        /// <summary>
        /// Set null to create a new user. Set user's Id to update a user
        /// </summary>
        public long? Id { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }


        /// <summary>
        /// 姓氏    【可不使用】
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [StringLength(User.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        // Not used "Required" attribute since empty value is used to 'not change password'
        [StringLength(User.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 下次登录需要修改密码
        /// </summary>
        public bool ShouldChangePasswordOnNextLogin { get; set; }

        /// <summary>
        /// 是否启用双重验证
        /// </summary>
        //public virtual bool IsTwoFactorEnabled { get; set; }

        /// <summary>
        /// 是否启用锁定
        /// </summary>
        public virtual bool IsLockoutEnabled { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(EmailAddress) && string.IsNullOrWhiteSpace(PhoneNumber))
            {
                context.Results.Add(new ValidationResult("手机号和邮箱不能同时为空"));
            }
        }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Surname))
            {
                Surname = "Surname";
            }
        }

    }
}

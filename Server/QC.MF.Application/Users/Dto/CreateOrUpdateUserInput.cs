using System.ComponentModel.DataAnnotations;

namespace QC.MF.Users.Dto
{
    /// <summary>
    /// 创建或编辑用户 参数
    /// </summary>
    public class CreateOrUpdateUserInput
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [Required]
        public UserEditDto User { get; set; }
        
        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        public string[] AssignedRoleNames { get; set; }

        /// <summary>
        /// 是否需要发送通知短信
        /// </summary>
        public bool SendActivationMessage { get; set; }

        /// <summary>
        /// 是否需要发送激活邮件
        /// </summary>
        public bool SendActivationEmail { get; set; }

        /// <summary>
        /// 是否使用随机密码
        /// </summary>
        public bool SetRandomPassword { get; set; }

        /// <summary>
        /// 组织机构信息
        /// </summary>
        public int[] Organizations { get; set; }
    }
}

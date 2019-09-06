using Abp.AutoMapper;

namespace QC.MF.Authorization.ThirdParty.Dto
{

    /// <summary>
    /// 第三方用户信息
    /// </summary>
    public class ThirdPartyUserOutput
    {
        /// <summary>
        /// 关联用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
    }
}

using Abp.Runtime.Validation;
using QC.MF.CommonDto;

namespace QC.MF.Users.Dto
{
    /// <summary>
    /// 获取用户列表参数
    /// </summary>
    public class GetUsersInput : PagedAndSortedInputDto, IShouldNormalize
    {

        /// <summary>
        /// 模糊匹配  Name、Surname、UserName、EmailAddress
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 拥有此权限的用户
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// 拥有此角色的用户
        /// </summary>
        public int? Role { get; set; }
        /// <summary>
        /// 姓名      
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name,Surname";
            }
        }
    }
}

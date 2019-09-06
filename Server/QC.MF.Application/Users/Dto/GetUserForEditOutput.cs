using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;

namespace QC.MF.Users.Dto
{
    /// <summary>
    /// 获取编辑用户信息
    /// </summary>
    public class GetUserForEditOutput
    {
        /// <summary>
        /// 头像ID
        /// </summary>
        public Guid? ProfilePictureId { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserEditDto User { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        public UserRoleDto[] Roles { get; set; }
        /// <summary>
        /// 组织机构信息
        /// </summary>
        public List<long> OrganizationIds { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetUserForEditOutput()
        {
            OrganizationIds = new List<long>();
        }
    }
}

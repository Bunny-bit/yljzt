using System.Collections.Generic;
using Abp.Application.Services.Dto;
using QC.MF.Authorization.Permissions.Dto;

namespace QC.MF.Roles.Dto
{
    /// <summary>
    /// �༭��ɫ ��Ҫ����Ϣ
    /// </summary>
    public class GetRoleForEditOutput
    {
        /// <summary>
        /// ��ɫ
        /// </summary>
        public RoleEditDto Role { get; set; }

        /// <summary>
        /// Ȩ���б�
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }

        /// <summary>
        /// �ý�ɫӵ�е�Ȩ��
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}

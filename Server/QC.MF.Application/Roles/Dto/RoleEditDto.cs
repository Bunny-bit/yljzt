using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using QC.MF.Authorization.Roles;

namespace QC.MF.Roles.Dto
{
    /// <summary>
    /// ��ɫ��Ϣ
    /// </summary>
    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// ��ʾ��
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// �Ƿ���Ĭ�Ͻ�ɫ
        /// </summary>
        public bool IsDefault { get; set; }
    }
}

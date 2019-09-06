using System;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using QC.MF.CommonDto;

namespace QC.MF.OrganizationUnits.Dto
{
    /// <summary>
    /// 获取组织下人员信息请求
    /// </summary>
    public class GetOrganizationUnitUsersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 是否递归查找下级机构数据
        /// </summary>
        public bool IsRecursiveSearch { get; set; }

        /// <summary>
        /// 用户名称过滤条件
        /// </summary>
        public string NameFilter { get; set; }

        /// <summary>
        /// 排序条件
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "user.Name, user.Surname";
            }
            else if (Sorting.Contains("userName"))
            {
                Sorting = Sorting.Replace("userName", "user.userName");
            }
            else if (Sorting.Contains("addedTime"))
            {
                Sorting = Sorting.Replace("addedTime", "uou.creationTime");
            }
        }
    }
}

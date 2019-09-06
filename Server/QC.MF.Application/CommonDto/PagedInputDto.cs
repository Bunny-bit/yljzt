using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace QC.MF.CommonDto
{
    /// <summary>
    /// 分页 条件
    /// </summary>
    public class PagedInputDto : IPagedResultRequest
    {
        /// <summary>
        /// 每页数据条数
        /// </summary>
        [Range(1, AppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        /// <summary>
        /// 跳过数据条数
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}

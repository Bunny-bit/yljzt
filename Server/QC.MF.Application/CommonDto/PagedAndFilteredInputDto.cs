using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace QC.MF.CommonDto
{
    /// <summary>
    /// 分页过滤条件
    /// </summary>
    public class PagedAndFilteredInputDto : IPagedResultRequest
    {
        /// <summary>
        /// 每页数量
        /// </summary>
        [Range(1, AppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        /// <summary>
        /// 跳过数量
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedAndFilteredInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}

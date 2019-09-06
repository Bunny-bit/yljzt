using Abp.Application.Services.Dto;

namespace QC.MF.CommonDto
{
    /// <summary>
    /// 分页 排序 条件
    /// </summary>
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}

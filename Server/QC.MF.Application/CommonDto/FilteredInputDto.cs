namespace QC.MF.CommonDto
{
    /// <summary>
    /// 分页 排序 过滤 条件
    /// </summary>
    public class FilteredInputDto :IFilteredResultRequest
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
    }
}

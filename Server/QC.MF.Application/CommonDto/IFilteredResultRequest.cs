namespace QC.MF.CommonDto
{
    public interface IFilteredResultRequest
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        string Filter { get; set; }
    }
}

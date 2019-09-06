namespace QC.MF.Commons
{
    public interface ICheckDelete: ICanDelete
    {
        /// <summary>
        /// 判断是否可以被删除
        /// </summary>
        void CheckDelete();
    }
}

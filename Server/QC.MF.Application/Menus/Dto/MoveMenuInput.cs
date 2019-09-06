namespace QC.MF.Menus.Dto
{
    /// <summary>
    /// 移动菜单项
    /// </summary>
    public class MoveMenuInput
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 新的上级菜单
        /// </summary>
        public int NewParentId { get; set; }
        /// <summary>
        /// 菜单新顺序
        /// </summary>
        public int NewOrder { get; set; }
    }
}

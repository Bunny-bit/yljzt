namespace QC.MF.Menus.Dto
{
    /// <summary>
    /// 更新菜单信息
    /// </summary>
    public class UpdateMenuInput
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 图标class 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 菜单是否可见
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// 权限限制
        /// </summary>
        public string RequiredPermissionName { get; set; }
        /// <summary>
        /// 目标链接
        /// </summary>
        public string Url { get; set; }
    }
}

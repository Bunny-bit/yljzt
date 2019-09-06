using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace QC.MF.Menus.Dto
{
    /// <summary>
    /// 菜单
    /// </summary>
    [AutoMapFrom(typeof(Menu))]
    public class MenuDto:EntityDto<int>
    {
        /// <summary>
        /// 上级菜单
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name => Id.ToString();

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 图标名称
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 是否叶子节点
        /// </summary>
        public bool IsLeaf { get; set; }
        /// <summary>
        /// 菜单顺序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 菜单是否可用
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 菜单是否可见
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// 权限限制
        /// </summary>
        public string RequiredPermissionName { get; set; }
        /// <summary>
        /// 是否需要登录才可查看菜单
        /// </summary>
        public bool RequiresAuthentication { get; set; }
        /// <summary>
        /// 打开方式 "_blank", "_self", "_parent", "_top"或者iframe名字
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 目标链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否系统菜单
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<MenuDto> Items { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuDto()
        {
            Items = new List<MenuDto>();
        }
    }
}

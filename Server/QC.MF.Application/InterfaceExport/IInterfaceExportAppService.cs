using Abp.Application.Services;

namespace QC.MF.InterfaceExport
{
    /// <summary>
    /// 接口导出
    /// </summary>
    public interface IInterfaceExportAppService : IApplicationService
    {
        /// <summary>
        /// 导出React使用的TypeScript文件
        /// </summary>
        string GetReactDownloadUrl();
    }
}

using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.Auditing.Dto;
using DataExporting.Dto;
using QC.MF.Demos.Dto;

namespace QC.MF.Demos
{
    /// <summary>
    /// FileSetting内容管理
    /// </summary>
    public interface IFileSettingDemoAppService:IApplicationService
    {
        /// <summary>
        /// 获取内容
        /// </summary>
        /// <returns></returns>
        Task<GetFileSettingDemoDto> Get();

        /// <summary>
        /// 修改内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Set(SetFileSettingDemoDto input);
    }
}

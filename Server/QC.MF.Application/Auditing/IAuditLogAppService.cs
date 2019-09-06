using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.Auditing.Dto;
using DataExporting.Dto;

namespace QC.MF.Auditing
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);
        Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);
    }
}

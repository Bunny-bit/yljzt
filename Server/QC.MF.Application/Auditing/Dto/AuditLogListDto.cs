using System;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using DataExporting.DataImportExporter;

namespace QC.MF.Auditing.Dto
{
    [ExportInfo("审计日志")]
    [AutoMapFrom(typeof(AuditLog))]
    public class AuditLogListDto : EntityDto<long>
    {
        [ExportInfo("用户ID")]
        public long? UserId { get; set; }

        [ExportInfo("用户名")]
        public string UserName { get; set; }

        public int? ImpersonatorTenantId { get; set; }

        public long? ImpersonatorUserId { get; set; }

        [ExportInfo("ServiceName")]
        public string ServiceName { get; set; }

        [ExportInfo("MethodName")]
        public string MethodName { get; set; }

        public string Parameters { get; set; }

        [ExportInfo("执行时间")]
        public string ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        [ExportInfo("客户Ip")]
        public string ClientIpAddress { get; set; }

        public string ClientName { get; set; }

        [ExportInfo("浏览器")]
        public string BrowserInfo { get; set; }

        [ExportInfo("异常")]
        public string Exception { get; set; }

        public string CustomData { get; set; }
    }
}

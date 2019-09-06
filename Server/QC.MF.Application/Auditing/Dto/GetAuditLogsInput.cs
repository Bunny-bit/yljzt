using System;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Timing;
using Abp.Application.Services.Dto;

namespace QC.MF.Auditing.Dto
{
    public class GetAuditLogsInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        public string UserName { get; set; }
        
        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string BrowserInfo { get; set; }
        
        public bool? HasException { get; set; }

        public int? MinExecutionDuration { get; set; }
        
        public int? MaxExecutionDuration { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "ExecutionTime DESC";
            }

            if (Sorting.IndexOf("UserName", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                Sorting = "User." + Sorting;
            }
            else
            {
                Sorting = "AuditLog." + Sorting;
            }

            if (StartDate == default(DateTime))
            {
                StartDate = DateTime.Now.AddDays(-3);
            }
            if (EndDate == default(DateTime))
            {
                EndDate = DateTime.Now;
            }
        }
    }
}

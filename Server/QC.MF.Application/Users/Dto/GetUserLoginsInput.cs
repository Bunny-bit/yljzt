using System;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Timing;
using Abp.Application.Services.Dto;

namespace QC.MF.Users.Dto
{
    public class GetUserLoginsInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
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

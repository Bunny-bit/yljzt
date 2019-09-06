using Abp.Extensions;
using Abp.Runtime.Validation;
using QC.MF.CommonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppEditions.Dto
{
    public class GetAppEditionsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        public AppSearchType AppSearchType { get; set; }


        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id DESC";
            }
        }
    }
}

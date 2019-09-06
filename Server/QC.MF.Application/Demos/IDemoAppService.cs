using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.Auditing.Dto;
using DataExporting.Dto;
using QC.MF.Demos.Dto;
using System.Collections.Generic;
using QC.MF.CommonDto;

namespace QC.MF.Demos
{
    /// <summary>
    /// Demo
    /// </summary>
    public interface IDemoAppService:IAsyncCrudAppService<GetListDemoDto, int, PagedSortedAndFilteredInputDto, CreateDemoDto, UpdateDemoDto>
    {
    }
}

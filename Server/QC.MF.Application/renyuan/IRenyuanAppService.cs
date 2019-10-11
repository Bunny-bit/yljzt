using Abp.Application.Services;
using QC.MF.CommonDto;
using QC.MF.Demos.Dto;
using QC.MF.renyuan.Dto;
using QC.MF.Renyuan.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.renyuan
{
    public interface IRenyuanAppService : IAsyncCrudAppService<GetListRenyuanDto, int, PagedSortedAndFilteredInputDto, CreateRenyuanDto, UpdateRenyuanDto>
    {
    }
}

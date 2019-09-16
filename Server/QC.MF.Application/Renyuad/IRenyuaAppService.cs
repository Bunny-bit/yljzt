using Abp.Application.Services;
using QC.MF.Renyua.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using QC.MF.CommonDto;

namespace QC.MF.Renyua
{
    public interface IRenyuaAppService 
        : IAsyncCrudAppService<GetListRenyuaDto, int, PagedSortedAndFilteredInputDto, CreateRenyuaDto, UpdateRenyuaDto>
    {
    }
}

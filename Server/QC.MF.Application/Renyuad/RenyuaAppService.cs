using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QC.MF.Renyua.Dto;
using QC.MF.CommonDto;
using QC.MF.Renyua;
using Abp.Domain.Repositories;

namespace QC.MF.Renyuad
{
    public class RenyuaAppService : AsyncMFCrudAppService<Renyua1, GetListRenyuaDto, PagedSortedAndFilteredInputDto, CreateRenyuaDto, UpdateRenyuaDto>, IRenyuaAppService
    {
        public RenyuaAppService(IRepository<Renyua1, int> repository) : base(repository)
        {
        }
    }
}

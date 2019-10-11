using Abp.Domain.Repositories;
using QC.MF.CommonDto;
using QC.MF.Demos.Dto;
using QC.MF.renyuan;
using QC.MF.renyuan.Dto;
using QC.MF.Renyuan.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Renyuan
{
    public class RenyuanAppService : AsyncMFCrudAppService<Renyuan1, GetListRenyuanDto, PagedSortedAndFilteredInputDto, CreateRenyuanDto, UpdateRenyuanDto>, IRenyuanAppService
    {
        public RenyuanAppService(IRepository<Renyuan1, int> repository) : base(repository)
        {

        }

    }
}

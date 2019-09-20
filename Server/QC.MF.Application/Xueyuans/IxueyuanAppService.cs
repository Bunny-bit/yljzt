using Abp.Application.Services;
using QC.MF.CommonDto;
using QC.MF.Xueyuans.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Xueyuans
{
  public  interface IXueyuanAppService : IAsyncCrudAppService<GetListxueyuanDto, int, PagedSortedAndFilteredInputDto, CreatexueyuanDto, UpdatexueyuanDto>
    {
    }
}

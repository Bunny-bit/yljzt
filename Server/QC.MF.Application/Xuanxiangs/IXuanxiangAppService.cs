using Abp.Application.Services;
using QC.MF.CommonDto;
using QC.MF.Xuanxiangs.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Xuanxiangs
{
   public interface IXuanxiangAppService : IAsyncCrudAppService<GetListXuanxiangDto, int, PagedSortedAndFilteredInputDto, CreateXuanxiangDto, UpdateXuanxiangDto>
    {
    }
}

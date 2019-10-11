using QC.MF.CommonDto;
using QC.MF.Xuanxiangs.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Xuanxiangs
{
   public class XuanxiangAppService : AsyncMFCrudAppService<Xuanxiang, GetListXuanxiangDto, PagedSortedAndFilteredInputDto, CreateXuanxiangDto, UpdateXuanxiangDto>, IXuanxiangAppService
    {
        public XuanxiangAppService(Abp.Domain.Repositories.IRepository<Xuanxiang, int> repository) : base(repository)
        {
        }
    }
}

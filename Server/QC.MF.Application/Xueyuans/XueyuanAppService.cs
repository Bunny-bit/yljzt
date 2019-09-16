using Abp.Domain.Repositories;
using QC.MF.CommonDto;
using QC.MF.Xueyuan;
using QC.MF.Xueyuans.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Xueyuans
{
    public class XueyuanAppService : AsyncMFCrudAppService<Xueyuan.Xueyuan, GetListxueyuanDto, PagedSortedAndFilteredInputDto, CreatexueyuanDto, UpdatexueyuanDto>, IXueyuanAppService
    {
        public XueyuanAppService(IRepository<Xueyuan.Xueyuan, int> repository) : base(repository)
        {
        }
    }
}

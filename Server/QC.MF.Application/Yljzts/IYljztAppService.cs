using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.CommonDto;
using QC.MF.Demos.Dto;
using QC.MF.Yljzts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Yljzts
{
    public interface IYljztAppService:IAsyncCrudAppService<GetListYljztDto, int, PagedSortedAndFilteredInputDto, CreateYljztDto, UpdateYljztDto>
    {
        Task<PagedResultDto<GetListYljztDto>> GetDajuan(PagedSortedAndFilteredInputDto input);

        List<ZhengQueShuZuiGao> GetZhengQueShuZuiGao();
    }
}

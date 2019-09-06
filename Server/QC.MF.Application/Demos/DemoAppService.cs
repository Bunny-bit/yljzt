using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using QC.MF.Authorization;
using QC.MF.Authorization.Users;
using DataExporting;
using DataExporting.Dto;
using Abp.Application.Services;
using QC.MF.Demos.Dto;
using QC.MF.CommonDto;
using QC.MF.PreviousAndNexts;
using QC.MF.JPush;
using System;

namespace QC.MF.Demos
{
    public class DemoAppService : AsyncMFCrudAppService<Demo, GetListDemoDto, PagedSortedAndFilteredInputDto, CreateDemoDto, UpdateDemoDto>, IDemoAppService
    {
        public DemoAppService(
            IRepository<Demo, int> repository
            ) : base(repository)
        {
            DeletePermissionName = PermissionNames.Pages_DemoMangeDelete;
            CreatePermissionName = PermissionNames.Pages_DemoMangeCreate;
            UpdatePermissionName = PermissionNames.Pages_DemoMangeUpdate;
        }
        public override async Task<GetListDemoDto> Get(EntityDto<int> input)
        {
            var data = await Repository.GetAsync(input.Id);
            var r = data.MapTo<GetDemoDto>();
            r.PreviousAndNext = new PreviousAndNext<Demo>(Repository, r.Id);
            return r;
        }
    }
}

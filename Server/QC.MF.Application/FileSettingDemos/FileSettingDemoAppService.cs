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
using QC.MF.Demos;
using QC.MF.Demos.Dto;

namespace QC.MF.Demos
{
    public class FileSettingDemoAppService : AsyncCrudAppService<FileSettingDemo, GetFileSettingDemoDto>, IFileSettingDemoAppService
    {

        public FileSettingDemoAppService(
            IRepository<FileSettingDemo, int> repository
            ) : base(repository)
        {
        }

        public async Task<GetFileSettingDemoDto> Get()
        {
            var data = await Find();
            return data.MapTo<GetFileSettingDemoDto>();
        }
        [AbpAuthorize(PermissionNames.FileSettingDemoMange)]
        public async Task Set(SetFileSettingDemoDto input)
        {
            var data = await Find();
            input.MapTo(data);
        }
        private async Task<FileSettingDemo> Find()
        {
            var data = await Repository.GetAll().FirstOrDefaultAsync();
            if (data == null)
            {
                data = Repository.Insert(new FileSettingDemo
                {
                    FileSize = 10240000,
                    FileExtension = ".mp3,.mp4"
                });
            }
            return data;
        }
    }
}

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using QC.MF.Users.Dto;
using Abp.Linq.Extensions;

namespace QC.MF.Users
{
    [AbpAuthorize]
    public class UserLoginAppService : MFAppServiceBase, IUserLoginAppService
    {
        private readonly IRepository<UserLoginAttempt, long> _userLoginAttemptRepository;

        public UserLoginAppService(IRepository<UserLoginAttempt, long> userLoginAttemptRepository)
        {
            _userLoginAttemptRepository = userLoginAttemptRepository;
        }

        [DisableAuditing]
        public async Task<PagedResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts(GetUserLoginsInput input)
        {
            var userId = AbpSession.GetUserId();
            var query = _userLoginAttemptRepository.GetAll()
                .Where(n=> n.CreationTime >= input.StartDate)
                .Where(n=> n.CreationTime < input.EndDate)
                .Where(la => la.UserId == userId);

            var resultCount = await query.CountAsync();
            var results = await query
                .AsNoTracking()
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<UserLoginAttemptDto>(resultCount, results.MapTo<List<UserLoginAttemptDto>>());
        }
    }
}

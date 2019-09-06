using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.Users.Dto;

namespace QC.MF.Users
{
    /// <summary>
    /// 登录历史
    /// </summary>
    public interface IUserLoginAppService : IApplicationService
    {
        /// <summary>
        /// 获取登录历史
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts(GetUserLoginsInput input);
    }
}

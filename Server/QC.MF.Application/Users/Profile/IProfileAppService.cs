using System.Threading.Tasks;
using Abp.Application.Services;
using QC.MF.Users.Profile.Dto;
using Abp.Application.Services.Dto;

namespace QC.MF.Users.Profile
{
    public interface IProfileAppService : IApplicationService
    {
        /// <summary>
        /// 编辑前，获取用户的基本信息
        /// </summary>
        /// <returns></returns>
        Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();

        /// <summary>
        /// 编辑用户的基本信息
        /// </summary>
        /// <returns></returns>
        Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);

        /// <summary>
        /// 修改自己的密码 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangePassword(ChangePasswordInput input);

        /// <summary>
        /// 管理员修改别人的密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeUserPassword(ChangeUserPasswordInput input);

        /// <summary>
        /// 管理员重置别人的密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> ResetUserPassword(EntityDto<long> input);

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProfilePicture(UpdateProfilePictureInput input);
        
        /// <summary>
        /// 获取密码复杂性设置
        /// </summary>
        /// <returns></returns>
        Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting();
    }
}

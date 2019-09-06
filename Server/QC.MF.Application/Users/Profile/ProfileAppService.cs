using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Extensions;
using Abp.IO;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using QC.MF.Users.Profile.Dto;
using QC.MF.Configuration;
using QC.MF.Security;
using QC.MF.Storage;
using QC.MF.Timing;
using Newtonsoft.Json;
using QC.MF.Authorization;
using Abp.Application.Services.Dto;
using QC.MF.Authorization.Users;

namespace QC.MF.Users.Profile
{
    [AbpAuthorize]
    public class ProfileAppService : MFAppServiceBase, IProfileAppService
    {
        private readonly IAppFolders _appFolders;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITimeZoneService _timeZoneService;
        private readonly PasswordComplexityChecker _passwordComplexityChecker;

        public ProfileAppService(
            IAppFolders appFolders,
            IBinaryObjectManager binaryObjectManager,
            ITimeZoneService timezoneService,
            PasswordComplexityChecker passwordComplexityChecker)
        {
            _appFolders = appFolders;
            _binaryObjectManager = binaryObjectManager;
            _timeZoneService = timezoneService;
            _passwordComplexityChecker = passwordComplexityChecker;
        }

        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            var user = await GetCurrentUserAsync();
            var userProfileEditDto = user.MapTo<CurrentUserProfileEditDto>();

            if (Clock.SupportsMultipleTimezone)
            {
                userProfileEditDto.Timezone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

                var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                if (userProfileEditDto.Timezone == defaultTimeZoneId)
                {
                    userProfileEditDto.Timezone = string.Empty;
                }
            }

            return userProfileEditDto;
        }

        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            var user = await GetCurrentUserAsync();
            input.MapTo(user);
            CheckErrors(await UserManager.UpdateAsync(user));

            if (Clock.SupportsMultipleTimezone)
            {
                if (input.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, input.Timezone);
                }
            }
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            await CheckPasswordComplexity(input.NewPassword);

            var user = await GetCurrentUserAsync();
            user.ShouldChangePasswordOnNextLogin = false;
            CheckErrors(await UserManager.ChangePasswordAsync(user.Id, input.CurrentPassword, input.NewPassword));
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Edit)]
        public async Task ChangeUserPassword(ChangeUserPasswordInput input)
        {
            await CheckPasswordComplexity(input.NewPassword);

            var user = await UserManager.GetUserByIdAsync(input.UserId);
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
        }

        [AbpAuthorize(PermissionNames.Pages_Administration_Users_Edit)]
        public async Task<string> ResetUserPassword(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.ChangePasswordAsync(user, User.DefaultPassword));
            return User.DefaultPassword;
        }

        public async Task UpdateProfilePicture(UpdateProfilePictureInput input)
        {
            var tempProfilePicturePath = Path.Combine(_appFolders.TempFileDownloadFolder, input.FileName);

            byte[] byteArray;

            using (var fsTempProfilePicture = new FileStream(tempProfilePicturePath, FileMode.Open))
            {
                using (var bmpImage = new Bitmap(fsTempProfilePicture))
                {
                    var width = input.Width == 0 ? bmpImage.Width : input.Width;
                    var height = input.Height == 0 ? bmpImage.Height : input.Height;
                    var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

                    using (var stream = new MemoryStream())
                    {
                        bmCrop.Save(stream, bmpImage.RawFormat);
                        stream.Close();
                        byteArray = stream.ToArray();
                    }
                }
            }

            if (byteArray.LongLength > 102400) //100 KB
            {
                throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit"));
            }

            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());

            if (user.ProfilePictureId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await _binaryObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;

            FileHelper.DeleteIfExists(tempProfilePicturePath);
        }

        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
            var settingValue = await SettingManager.GetSettingValueAsync(AppSettingNames.Security.PasswordComplexity);
            var setting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(settingValue);

            return new GetPasswordComplexitySettingOutput
            {
                Setting = setting
            };
        }

        private async Task CheckPasswordComplexity(string password)
        {       
            _passwordComplexityChecker.Check( password);
        }
    }
}

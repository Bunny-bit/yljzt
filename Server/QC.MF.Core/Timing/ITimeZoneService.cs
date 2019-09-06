using System.Threading.Tasks;
using Abp.Configuration;

namespace QC.MF.Timing
{
    public interface ITimeZoneService
    {
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);
    }
}

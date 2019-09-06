using System.Threading.Tasks;
using Abp.Application.Services;
using QC.MF.Sessions.Dto;

namespace QC.MF.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}

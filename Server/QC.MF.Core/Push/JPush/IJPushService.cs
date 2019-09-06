using Abp.Domain.Services;

namespace QC.MF.JPush
{
    public interface IJPushService : IDomainService
    {
        void Push(string alert, PushParam content, string[] targets, PushType type = PushType.Alias);
    }
}

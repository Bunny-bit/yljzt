using Abp.Domain.Entities;

namespace QC.MF.Authorization.ThirdParty
{
    public class ThirdPartyUser:Entity<long>
    {
        public long UserId { get; set; }

        public string OpenId { get; set; }

        public string ThirdParty { get; set; }

        public string AccessToken { get; set; }
        public string Name { get; set; }

        public string NickName { get; set; }
    }
}

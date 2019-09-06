using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace QC.MF.Web.Models.Account
{
    public class LoginViewModel
    {
        public string TenancyName { get; set; }

        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string Captcha { get;set; }
    }
}

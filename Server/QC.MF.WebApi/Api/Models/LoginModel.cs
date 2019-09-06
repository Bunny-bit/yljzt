using System.ComponentModel.DataAnnotations;

namespace QC.MF.Api.Models
{
    public class LoginModel
    {
        public bool RememberMe { get; set; }

        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Captcha { get; set; }
    }
}

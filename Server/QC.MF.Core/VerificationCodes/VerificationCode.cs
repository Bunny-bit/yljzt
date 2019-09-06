using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.VerificationCodes
{
    public class VerificationCode : AuditedEntity
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        public bool IsVerifyPass { get; set; }

        public int ErrorCount { get; set; }

        public static string GetRandomCode()
        {
            return new Random().Next(1000000, 1999999).ToString().Substring(1);
        }
    }
}

using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.AppEditions.Dto
{
    public class VersionInput : ICustomValidate
    {
        public string Version { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            try
            {
                System.Version.Parse(Version);
            }
            catch (Exception e)
            {
                context.Results.Add(new ValidationResult(e.Message));
            }
        }
    }
}

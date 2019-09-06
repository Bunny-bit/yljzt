using Abp;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.WebFiles
{
    public class WebFile : AuditedEntity<Guid>
    {
        [StringLength(512)]
        public string FileName { get; set; }
        [StringLength(512)]
        public string FilePath { get; set; }
        [StringLength(512)]
        public string TempFilePath { get; set; }
        public WebFile()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace QC.MF.Configuration.Dto
{
    public class SkinInput
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
    }
}

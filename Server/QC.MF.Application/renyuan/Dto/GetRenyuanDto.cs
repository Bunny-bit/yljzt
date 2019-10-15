using Abp.AutoMapper;
using QC.MF.Demos.Dto;
using QC.MF.Renyuan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.renyuan.Dto
{
    [AutoMap(typeof(Renyuan1))]
    public class GetRenyuanDto: GetListRenyuanDto
    {
    }
}

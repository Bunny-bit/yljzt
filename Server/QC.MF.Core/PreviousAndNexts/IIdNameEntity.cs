using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF
{
    public interface IIdNameEntity  : IEntity
    {
        string Name { get; set; }
    }
}

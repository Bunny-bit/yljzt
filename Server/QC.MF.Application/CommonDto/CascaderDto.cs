using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace QC.MF.CommonDto
{
    /// <summary>
    /// 级联选择 antd组件
    /// </summary>
    public class CascaderDto<T>
    {
        public string Label { get; set; }
        public T Value { get; set; }
        public IEnumerable<CascaderDto<T>> Children { get; set; }
    }

    public class CascaderDto : CascaderDto<int>
    {
    }
    
}

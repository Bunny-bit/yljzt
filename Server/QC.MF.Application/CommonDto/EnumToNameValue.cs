using Abp.Application.Services.Dto;
using QC.MF.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace QC.MF.CommonDto
{
    public static class EnumToNameValue
    {
        public static IEnumerable<NameValueDto<int>> EnumToNameValueDto<T>()
        {
            var type = typeof(T);
            if (type.IsEnum)
            {
                foreach (Enum item in type.GetEnumValues())
                {
                    var name = item.GetText();
                    yield return new NameValueDto<int>(name, (int)(object)item);
                }
            }
        }
    }
}

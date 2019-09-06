using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF
{
    public static class DynamicQueryableExtensions
    {
        /// <summary>
        /// 是否是比较过滤
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static bool HasRecognizable(string filter) =>
            ConnectionSymbol.Any(x => filter.Contains(x)) ||
            CompareSymbol.Any(x => filter.Contains(x));

        /// <summary>
        /// 动态过滤
        /// </summary>
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, string filter)
        {
            if (!HasRecognizable(filter))
            {
                return query;
            }
            var dataType = query.GetType().GetGenericArguments()[0];
            var predicates = AnalysisFilter(filter, dataType);
            foreach (var item in predicates)
            {
                if (string.IsNullOrEmpty(item.predicate))
                {
                    continue;
                }
                if (item.values == null)
                {
                    query = query.Where(item.predicate);
                }
                else
                {
                    query = query.Where(item.predicate, item.values);
                }
            }
            return query;
        }
        public static List<(string predicate, object[] values)> AnalysisFilter(string filter, Type dataType)
        {
            var predicates = new List<(string predicate, object[] values)>();
            if (string.IsNullOrEmpty(filter))
            {
                return predicates;
            }
            var filters = filter.Split('&');
            foreach (var item in filters)
            {
                var compareSymbol = GetCompareSymbol(item);
                if (string.IsNullOrEmpty(compareSymbol))
                {   // 没有包含比较符号，跳过
                    continue;
                }
                var nameValue = item.Split(new string[] { compareSymbol }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                if (nameValue.Length != 2)
                {
                    continue;
                }

                var operates = new List<string>();
                var valueType = dataType;
                foreach (var _name in nameValue[0].Split(','))
                {
                    string name = _name.toProperCase();
                    if (name.Contains("."))
                    {
                        var parts = name.ToStringList('.');
                        bool isNameCorrect = true;
                        valueType = dataType;
                        foreach (var part in parts)
                        {
                            var property = valueType.GetProperties().FirstOrDefault(x => x.Name == part.toProperCase());
                            if (property == null)
                            {
                                isNameCorrect = false;
                                break;
                            }
                            valueType = property.PropertyType;
                        }
                        if (!isNameCorrect)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (dataType.GetProperties().FirstOrDefault(x => x.Name == name) == null)
                        {   // 错误的属性名称
                            continue;
                        }
                        valueType = dataType.GetProperties().FirstOrDefault(x => x.Name == name.toProperCase()).PropertyType;
                    }
                    operates.Add(name + CompareSymbolFunction.First(x => x.symbol == compareSymbol).fun);
                }
                if (operates.Any())
                {
                    var value = ConvertType(nameValue[1], valueType);
                    predicates.Add((string.Join("||", operates), new object[] { value }));
                }
            }
            return predicates;
        }
        private static object AsObject(string pro, string strValue, Type type)
        {
            if (pro.Contains("."))
            {
                var parts = pro.ToStringList('.');
                foreach (var part in parts)
                {
                    var property = type.GetProperties().FirstOrDefault(x => x.Name == part.toProperCase());
                    if (property == null)
                        return null;
                    type = property.PropertyType;
                }
                return ConvertType(strValue, type);
            }
            else
            {
                var proType = type.GetProperties().FirstOrDefault(x => x.Name == pro).PropertyType;
                return ConvertType(strValue, proType);
            }
        }
        public static object ConvertType(string strValue, Type proType)
        {
            if (strValue == "null")
            {
                return null;
            }
            if (proType.FullName.Contains("System.Guid"))
            {
                return new Guid(strValue);
            }
            if (proType.IsEnum)
            {
                return Enum.Parse(proType, strValue);
            }
            if (!proType.IsGenericType)
            {
                return Convert.ChangeType(strValue, proType);
            }
            else
            {
                Type genericTypeDefinition = proType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return Convert.ChangeType(strValue, Nullable.GetUnderlyingType(proType));
                }
                else
                {
                    throw new Abp.UI.UserFriendlyException("未能处理该请求，请联系开发人员处理");
                }
            }
        }
        private static string GetCompareSymbol(string predicate)
        {
            foreach (var item in CompareSymbol)
            {
                if (predicate.Contains(item))
                {
                    return item;
                }
            }
            return "";
        }

        /// <summary>
        /// 比较符号
        /// </summary>
        static List<(string symbol, string fun)> CompareSymbolFunction = new List<(string, string)>
        {
            ("like",".Contains(@0)"),("!=","<>@0"),("==","=@0"),(">=",">=@0"),("<=","<=@0"),("<","<@0"),(">",">@0")
        };

        /// <summary>
        /// 比较符号
        /// </summary>
        static IEnumerable<string> CompareSymbol = CompareSymbolFunction.Select(x => x.symbol);

        /// <summary>
        /// 连接符号
        /// </summary>
        static List<string> ConnectionSymbol = new List<string>
        {
            "&"
        };


        private static string toProperCase(this string s)
        {
            string revised = "";
            if (s.Length > 0)
            {
                revised = s.Trim();
                revised = revised.Substring(0, 1).ToUpper() + revised.Substring(1);
            }
            return revised;
        }
    }
}

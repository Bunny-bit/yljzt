using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Abp.Extensions;

namespace QC.MF
{
    public static class StringExtensions
    {
        /// <summary>
        /// String转换为String List
        /// </summary>
        public static List<string> ToStringList(this string str, params char[] spliterArr)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<string>();
            }
            if (spliterArr == null || spliterArr.Length == 0) spliterArr = new[] { ',' };
            var spliters = new List<char>(spliterArr);
            foreach (var spliter in spliterArr)
            {
                if (spliter < 127)
                {
                    spliters.Add(spliter.ToSBC());
                }
                else
                {
                    spliters.Add(spliter.ToDBC());
                }
            }
            return str.Split(spliters.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// String转换为String Array
        /// </summary>
        public static string[] ToStringArray(this string str, params char[] spliterArr)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new string[] { };
            }
            if (spliterArr == null || spliterArr.Length == 0) spliterArr = new[] { ',' };
            var spliters = new List<char>(spliterArr);
            foreach (var spliter in spliterArr)
            {
                if (spliter < 127)
                {
                    spliters.Add(spliter.ToSBC());
                }
                else
                {
                    spliters.Add(spliter.ToDBC());
                }
            }
            return str.Split(spliters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }


        /// 转全角的函数(SBC case)
        /// 任意字符串 
        /// 全角字符串
        ///全角空格为12288,半角空格为32 
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        public static string ToSBC(this string input)
        { //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288; continue;
                }
                if (c[i] < 127) c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// 转半角的函数(DBC case)
        ///任意字符串
        /// 半角字符串
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248 ///
        public static char ToDBC(this char input)
        {
            if (input == 12288)
            {
                input = (char)32;
            }
            if (input > 65280 && input < 65375)
            {
                input = (char)(input - 65248);
            }

            return input;
        }

        /// 转半角的函数(DBC case)
        ///任意字符串
        /// 半角字符串
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248 ///
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32; continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// String转换为Int List
        /// </summary>
        public static List<int> ToIntList(this string intStr, params char[] spliterArr)
        {
            if (string.IsNullOrEmpty(intStr))
            {
                return new List<int>();
            }
            if (spliterArr == null || spliterArr.Length == 0) spliterArr = new[] { ',' };
            var spliters = new List<char>(spliterArr);
            foreach (var spliter in spliterArr)
            {
                if (spliter < 127)
                {
                    spliters.Add(spliter.ToSBC());
                }
                else
                {
                    spliters.Add(spliter.ToDBC());
                }
            }
            return intStr.Split(spliters.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(ToInt)
                .ToList();
        }


        /// 转全角的函数(SBC case)
        /// 任意字符串 
        /// 全角字符串
        ///全角空格为12288,半角空格为32 
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        public static char ToSBC(this char input)
        { //半角转全角：
            if (input == 32)
            {
                input = (char)12288;
            }
            if (input < 127)
            {
                input = (char)(input + 65248);
            }
            return input;
        }

        public static int ToInt(this string c, int defValue = 0)
        {
            if (string.IsNullOrWhiteSpace(c))
            {
                return defValue;
            }
            int rv;
            return int.TryParse(c, out rv) ? rv : 0;
        }

        public static DateTime ToDatetime(this string c)
        {
            if (string.IsNullOrWhiteSpace(c))
            {
                return DateTime.MinValue;
            }
            c = c.TrimEnd('_', ':');
            DateTime val;
            return DateTime.TryParse(c, out val) ? val : DateTime.MinValue;
        }

        public static decimal ToDecimal(this string c, decimal defValue=0)
        {
            if (string.IsNullOrWhiteSpace(c))
            {
                return defValue;
            }
            c = c.Replace(",", "");
            decimal rv;
            return decimal.TryParse(c, out rv) ? rv : 0;
        }

        public static string ExtractPhoneNumber(this string phoneNumber)
        {
            const string MobileRegex = "(13[0-9]|15[012356789]|17[013678]|18[0-9]|14[57])[0-9]{8}";
            var match = Regex.Match(phoneNumber, MobileRegex);
            if (!match.Success)
            {
                return "";
            }
            return match.Value;
        }
        /// <summary>
        /// 是否手机访问
        /// </summary>
        /// <returns></returns>
        public static bool IsPhoneNumber(this string phoneNumber)
        {
            return !phoneNumber.ExtractPhoneNumber().IsNullOrEmpty();
        }
    }
}

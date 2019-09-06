using System;
using System.Collections.Generic;

namespace QC.MF.Captcha
{
    public static class CodeMaker
    {
        private static readonly List<char> CharList = new List<char>
        {
            /*'1',*/ '2', '3', '4', '5', '6', '7', '8', '9', /*'0',*/
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', /*'l',*/ 'm', 'n', /*'o',*/ 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', /*'O',*/ 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static string MakeCode(int length = 4)
        {
            var chars = RandomTakeItem(CharList, length);
            return string.Join("", chars);
        }

        /// <summary>
        /// 随机取列表中的Item
        /// </summary>
        public static List<T> RandomTakeItem<T>(List<T> source, int count)
        {
            if (source == null || source.Count <= count)
            {
                return source;
            }
            var result = new List<T>();
            var random = new Random();
            var workList = source.GetRange(0, source.Count);
            for (int i = 0; i < count; i++)
            {
                var index = random.Next(i, workList.Count);
                result.Add(workList[index]);
                workList[index] = workList[i];
            }
            return result;
        }
    }
}

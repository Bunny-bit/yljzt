using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF
{

    public class Grouping<TKey, TElement> 
    {
        public TKey Key { get; set; }
        public IEnumerable<TElement> Data { get; set; }

    }

    public static class Grouping
    {
        public static  Grouping<TKey, TElement>ToGrouping<TKey, TElement>(this IGrouping<TKey, TElement> data)
        {
            return new Grouping<TKey, TElement>() 
            {
                Key=data.Key,
                Data=data
            };
        }
        public static IEnumerable<Grouping<TKey, TElement>>ToGrouping<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> data)
        {
            return data.Select(x => x.ToGrouping());            
        }
    }
}

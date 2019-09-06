using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Commons
{

    public class TextAttribute : Attribute
    {
        public TextAttribute(string text)
        {
            Text = text;
        }
        public string Text { get; set; }
    }
    public static class EnumExtentions
    {
        public static string GetText(this Enum t)
        {
            var t_type = t.GetType();
            var fieldName = Enum.GetName(t_type, t);
            var attributes = t_type.GetField(fieldName).GetCustomAttributes(false);
            var enumDisplayAttribute = attributes.FirstOrDefault(p => p.GetType().Equals(typeof(TextAttribute))) as TextAttribute;
            return enumDisplayAttribute == null ? fieldName : enumDisplayAttribute.Text;
        }
    }
}

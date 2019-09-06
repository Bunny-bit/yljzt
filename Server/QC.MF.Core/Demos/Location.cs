using System.ComponentModel.DataAnnotations.Schema;

namespace QC.MF.Demos
{
    /// <summary>
    /// 位置信息
    /// </summary>
    [ComplexType]
    public class Location
    {
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Latitude { get; set; }
    }
}

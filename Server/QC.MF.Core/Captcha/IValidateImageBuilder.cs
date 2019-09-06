using System.Drawing;

namespace QC.MF.Captcha
{
    internal interface IValidateImageBuilder
    {
        Image CreateImage(string code);
        int Height { get; set; }
        int Width { get; set; }
        int Difficulty { get; }
    }
}

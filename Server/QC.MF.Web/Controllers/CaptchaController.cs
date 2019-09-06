using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using QC.MF.Captcha;
using DataExporting.Net.MimeTypes;

namespace QC.MF.Web.Controllers
{
    public class CaptchaController : Controller
    {
        private readonly ICaptchaManager _captchaManager;
        public CaptchaController(ICaptchaManager captchaManager)
        {
            _captchaManager = captchaManager;
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns></returns>
        public FileResult GetCaptchaImage()
        {
            var image = _captchaManager.GetCaptchaImage();
            var imgStream = new MemoryStream();
            image.Save(imgStream, ImageFormat.Jpeg);
            imgStream.Seek(0, SeekOrigin.Begin);
            return File(imgStream, MimeTypeNames.ImageJpeg);
        }
    }
}

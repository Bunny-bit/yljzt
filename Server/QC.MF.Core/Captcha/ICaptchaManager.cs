using System.Drawing;

namespace QC.MF.Captcha
{
    public interface ICaptchaManager
    {
        /// <summary>
        ///  验证验证码
        /// </summary>
        /// <param name="inputCaptcha"></param>
        void CheckCaptcha(string inputCaptcha);

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns></returns>
        Image GetCaptchaImage(int width=80, int height=40);
    }
}

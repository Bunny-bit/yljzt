using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using QC.MF.Captcha;

namespace QC.MF.Captcha
{
    public class ValidateImageBuilderOne : IValidateImageBuilder
    {
        /// <summary>
        /// 验证码的最大长度
        /// </summary>
        public int MaxLength
        {
            get { return 10; }
        }
        /// <summary>
        /// 验证码的最小长度
        /// </summary>
        public int MinLength
        {
            get { return 1; }
        }

        public int Height { get; set; }
        public int Width { get; set; }

        public int Difficulty
        {
            get { return 1; }
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = seekRand.Next(0, int.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode">验证码</param>
        public Image CreateImage(string validateCode)
        {
            //创建画板
            Bitmap image = new Bitmap(GetImageWidth(validateCode.Length), 30);
            Random rand = new Random();
            Graphics g = Graphics.FromImage(image);

            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.InterpolationMode = InterpolationMode.Low;
            //g.CompositingMode = CompositingMode.SourceOver;
            //g.CompositingQuality = CompositingQuality.HighQuality;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //绘制渐变背景
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            Brush brushBack = new LinearGradientBrush(rect, Color.FromArgb(rand.Next(150, 256), 255, 255), Color.FromArgb(255, rand.Next(150, 256), 255), rand.Next(90));
            g.FillRectangle(brushBack, rect);

            //绘制干扰曲线
            for (int i = 0; i < 2; i++)
            {
                Point p1 = new Point(0, rand.Next(image.Height));
                Point p2 = new Point(rand.Next(image.Width), rand.Next(image.Height));
                Point p3 = new Point(rand.Next(image.Width), rand.Next(image.Height));
                Point p4 = new Point(image.Width, rand.Next(image.Height));
                Point[] p = { p1, p2, p3, p4 };
                Pen pen = new Pen(Color.Gray, 1);
                g.DrawBeziers(pen, p);
            }

            //逐个绘制文字
            for (int i = 0; i < validateCode.Length; i++)
            {
                string strChar = validateCode.Substring(i, 1);
                int deg = rand.Next(-15, 15);
                float x = (image.Width / validateCode.Length / 2) + (image.Width / validateCode.Length) * i;
                float y = image.Height / 2.0f;
                //随机字体大小
                Font font = new Font("Consolas", rand.Next(16, 24), FontStyle.Regular);
                SizeF size = g.MeasureString(strChar, font);
                Matrix m = new Matrix();
                //旋转
                m.RotateAt(deg, new PointF(x, y), MatrixOrder.Append);
                //扭曲
                m.Shear(rand.Next(-10, 10) * 0.03f, 0);
                g.Transform = m;
                //随机渐变画笔
                Brush brushPen = new LinearGradientBrush(rect, Color.FromArgb(rand.Next(0, 256), 0, 0), Color.FromArgb(0, 0, rand.Next(0, 256)), rand.Next(90));
                g.DrawString(validateCode.Substring(i, 1), font, brushPen, new PointF(x - size.Width / 2, y - size.Height / 2));

                g.Transform = new Matrix();
            }
            g.Save();
            return image;
        }
        /// <summary>
        /// 得到验证码图片的长度
        /// </summary>
        /// <param name="validateNumLength">验证码的长度</param>
        /// <returns></returns>
        public static int GetImageWidth(int validateNumLength)
        {
            return (int)(validateNumLength * 22.5);
        }
    }
}


using System;
using System.Drawing;
using QC.MF.Captcha;

namespace QC.MF.Captcha
{
    public class ValidateImageBuilderTwo : IValidateImageBuilder
    {

        public int Height { get; set; }
        public int Width { get; set; }

        public int Difficulty
        {
            get { return 2; }
        }

        /// <summary>
        /// 创建随机码图片
        /// </summary>
        /// <param name="randomcode">随机码</param>
        public Image CreateImage(string randomcode)
        {
            int randAngle = 45; //随机转动角度
            int mapwidth = (int)(randomcode.Length * 16);
            Bitmap map = new Bitmap(mapwidth, 22);//创建图片背景
            Graphics graph = Graphics.FromImage(map);
            graph.Clear(Color.AliceBlue);//清除画面，填充背景
            graph.DrawRectangle(new Pen(Color.Black, 0), 0, 0, map.Width - 1, map.Height - 1);//画一个边框
                                                                                              //graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//模式

            Random rand = new Random();

            //背景噪点生成
            Pen blackPen = new Pen(Color.LightGray, 0);
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(0, map.Width);
                int y = rand.Next(0, map.Height);
                graph.DrawRectangle(blackPen, x, y, 1, 1);
            }


            //验证码旋转，防止机器识别
            char[] chars = randomcode.ToCharArray();//拆散字符串成单字符数组

            //文字距中
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            int cindex = rand.Next(7);

            for (int i = 0; i < chars.Length; i++)
            {
                int findex = rand.Next(5);

                Font f = new System.Drawing.Font(font[findex], 14, System.Drawing.FontStyle.Bold);//字体样式(参数2为字体大小)
                Brush b = new System.Drawing.SolidBrush(c[cindex]);

                Point dot = new Point(14, 14);
                //graph.DrawString(dot.X.ToString(),fontstyle,new SolidBrush(Color.Black),10,150);//测试X坐标显示间距的
                float angle = rand.Next(-randAngle, randAngle);//转动的度数

                graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置
                graph.RotateTransform(angle);
                graph.DrawString(chars[i].ToString(), f, b, 1, 1, format);
                //graph.DrawString(chars[i].ToString(),fontstyle,new SolidBrush(Color.Blue),1,1,format);
                graph.RotateTransform(-angle);//转回去
                graph.TranslateTransform(-2, -dot.Y);//移动光标到指定位置，每个字符紧凑显示，避免被软件识别
            }
            return map;
        }
    }
}

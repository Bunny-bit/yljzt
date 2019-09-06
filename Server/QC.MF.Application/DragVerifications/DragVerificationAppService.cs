using System;
using System.Linq;
using System.Drawing;
using System.IO;
using QC.MF.DragVerifications.Dto;
using System.Web;
using Abp.Runtime.Caching;
using QC.MF.Captcha;

namespace QC.MF.DragVerifications
{
    public class DragVerificationAppService : MFAppServiceBase, IDragVerificationAppService
    {
        private readonly IAppFolders _appFolders;
        private readonly ICacheManager _cacheManager;
        //裁剪的小图大小
        private const int _shearSize = 40;
        //原始图片宽px
        private int _ImgWidth = 300;
        //原始图片高px
        private int _ImgHeight = 300;
        //裁剪位置X轴最小位置
        private int _MinRangeX = 30;
        //裁剪位置X轴最大位置
        private int _MaxRangeX = 240;
        //裁剪位置Y轴最小位置
        private int _MinRangeY = 30;
        //裁剪位置Y轴最大位置
        private int _MaxRangeY = 200;
        //裁剪X轴大小 裁剪成20张上10张下10张
        private int _CutX = 30;
        //裁剪Y轴大小 裁剪成20张上10张下10张
        private int _CutY = 150;
        //小图相对原图左上角的x坐标  x坐标保存到session 用于校验
        //允许误差 单位像素
        private const int _deviationPx = 2;
        //最大错误次数
        private const int _MaxErrorNum = 4;
        public DragVerificationAppService(IAppFolders appFolders,
             ICacheManager cacheManager)
        {
            _appFolders = appFolders;
            _cacheManager = cacheManager;
        }

        private void SetCache(VerifcationCache verifcationCache)
        {
            var requestCookie = HttpContext.Current.Request.Cookies.Get("ClientToken");
            var clientToken = "";
            if (requestCookie == null)
            {
                clientToken = Guid.NewGuid().ToString("N");
                var cookie = new HttpCookie("ClientToken", clientToken);
                HttpContext.Current.Response.SetCookie(cookie);
            }
            else
            {
                clientToken = requestCookie.Value;
            }
            if (verifcationCache != null)
            {
                _cacheManager.GetCache("ClientToken").Set(clientToken, verifcationCache);
            }
            else
            {
                _cacheManager.GetCache("ClientToken").Remove(clientToken);
            }
        }

        private VerifcationCache GetCache()
        {
            var requestCookie = HttpContext.Current.Request.Cookies.Get("ClientToken");
            if (requestCookie == null)
            {
                throw new Abp.UI.UserFriendlyException("错误的请求");
            }
            var clientToken = requestCookie.Value;
            var verifcationCache = _cacheManager.GetCache("ClientToken").GetOrDefault<string, VerifcationCache>(clientToken);
            if (verifcationCache?.VerifcationType != VerifcationType.Drag)
            {
                throw new Abp.UI.UserFriendlyException("错误的请求");
            }
            return verifcationCache;
        }

        public DragVerificationDto GetDragVerificationCode()
        {

            Random rd = new Random();
            int _PositionX = rd.Next(_MinRangeX, _MaxRangeX);
            int _PositionY = rd.Next(_MinRangeY, _MaxRangeY);
            SetCache(new VerifcationCache()
            {
                VerifcationType = VerifcationType.Drag,
                ErrorCount = 0,
                Code = _PositionX.ToString()
            });
            int[] array = Enumerable.Range(0, 20).OrderBy(x => Guid.NewGuid()).ToArray();
            var files = Directory.GetFiles(_appFolders.DragVerificationImageFolder).Where(n => n.ToLower().EndsWith(".jpg")).ToArray();
            Bitmap bmp = new Bitmap(files[rd.Next(files.Length)]);
            string ls_small = "data:image/jpg;base64," + ImgToBase64String(cutImage(bmp, _shearSize, _shearSize, _PositionX, _PositionY));
            Bitmap lb_normal = GetNewBitMap(bmp, _shearSize, _shearSize, _PositionX, _PositionY);
            string ls_confusion = "data:image/jpg;base64," + ImgToBase64String(ConfusionImage(array, lb_normal));


            return new DragVerificationDto()
            {
                Y = _PositionY,
                Array = array,
                ImgX = _ImgWidth,
                ImgY = _ImgHeight,
                Small = ls_small,
                Normal = ls_confusion
            };
        }

        public CheckCodeOutput CheckCode(CheckCodeInput input)
        {
            var verifcationCache = GetCache();
            var old_point = int.Parse(verifcationCache.Code);
            //错误
            if (Math.Abs(old_point - input.Point) > _deviationPx)
            {
                verifcationCache.ErrorCount++;
                if (verifcationCache.ErrorCount > _MaxErrorNum)
                {
                    //超过最大错误次数后不再校验
                    SetCache(null);
                    throw new Abp.UI.UserFriendlyException("错误的次数太多，请刷新重试");
                }
                SetCache(verifcationCache);
                //返回错误次数
                return new CheckCodeOutput() { Success = false };
            }
            if (!SlideFeature(input.DateList))
            {
                //机器人??
                throw new Abp.UI.UserFriendlyException("您的操作有误，请重新拖动");
            }
            //校验成功 返回正确坐标

            verifcationCache.Code = Guid.NewGuid().ToString();
            SetCache(verifcationCache);

            return new CheckCodeOutput() { Success = true, Token = verifcationCache.Code };
        }

        /// <summary>
        /// 获取裁剪的小图
        /// </summary>
        /// <param name="sFromBmp">原图</param>
        /// <param name="cutWidth">剪切宽度</param>
        /// <param name="cutHeight">剪切高度</param>
        /// <param name="x">X轴剪切位置</param>
        /// <param name="y">Y轴剪切位置</param>
        private Bitmap cutImage(Bitmap sFromBmp, int cutWidth, int cutHeight, int x, int y)
        {
            //载入底图   
            Image fromImage = sFromBmp;

            //先初始化一个位图对象，来存储截取后的图像
            Bitmap bmpDest = new Bitmap(cutWidth, cutHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            //这个矩形定义了，你将要在被截取的图像上要截取的图像区域的左顶点位置和截取的大小
            Rectangle rectSource = new Rectangle(x, y, cutWidth, cutHeight);

            //这个矩形定义了，你将要把 截取的图像区域 绘制到初始化的位图的位置和大小
            //我的定义，说明，我将把截取的区域，从位图左顶点开始绘制，绘制截取的区域原来大小
            Rectangle rectDest = new Rectangle(0, 0, cutWidth, cutHeight);

            //第一个参数就是加载你要截取的图像对象，第二个和第三个参数及如上所说定义截取和绘制图像过程中的相关属性，第四个属性定义了属性值所使用的度量单位
            Graphics g = Graphics.FromImage(bmpDest);
            g.DrawImage(fromImage, rectDest, rectSource, GraphicsUnit.Pixel);
            g.Dispose();
            return bmpDest;
        }
        /// <summary>
        /// 获取裁剪小图后的原图
        /// </summary>
        /// <param name="sFromBmp">原图</param>
        /// <param name="cutWidth">剪切宽度</param>
        /// <param name="cutHeight">剪切高度</param>
        /// <param name="spaceX">X轴剪切位置</param>
        /// <param name="spaceY">Y轴剪切位置</param>
        private Bitmap GetNewBitMap(Bitmap sFromBmp, int cutWidth, int cutHeight, int spaceX, int spaceY)
        {
            // 加载原图片 
            Bitmap oldBmp = sFromBmp;
            // 绑定画板 
            Graphics grap = Graphics.FromImage(oldBmp);
            // 加载水印图片 
            Bitmap bt = new Bitmap(cutWidth, cutHeight);
            Graphics g1 = Graphics.FromImage(bt);  //创建b1的Graphics
            g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, cutWidth, cutHeight));   //把b1涂成红色
            bt = PTransparentAdjust(bt, 120);
            // 添加水印 
            grap.DrawImage(bt, spaceX, spaceY, cutWidth, cutHeight);
            grap.Dispose();
            g1.Dispose();
            return oldBmp;
        }
        /// <summary>
        /// 获取半透明图像
        /// </summary>
        /// <param name="bmp">Bitmap对象</param>
        /// <param name="alpha">alpha分量。有效值为从 0 到 255。</param>
        private Bitmap PTransparentAdjust(Bitmap bmp, int alpha)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color bmpcolor = bmp.GetPixel(i, j);
                    byte A = bmpcolor.A;
                    byte R = bmpcolor.R;
                    byte G = bmpcolor.G;
                    byte B = bmpcolor.B;
                    bmpcolor = Color.FromArgb(alpha, R, G, B);
                    bmp.SetPixel(i, j, bmpcolor);
                }
            }
            return bmp;
        }
        /// <summary>
        /// 获取混淆拼接的图片
        /// </summary>
        /// <param name="a">无序数组</param>
        /// <param name="bmp">剪切小图后的原图</param>
        private Bitmap ConfusionImage(int[] a, Bitmap cutbmp)
        {
            Bitmap[] bmp = new Bitmap[20];
            for (int i = 0; i < 20; i++)
            {
                int x, y;
                x = a[i] > 9 ? (a[i] - 10) * _CutX : a[i] * _CutX;
                y = a[i] > 9 ? _CutY : 0;
                bmp[i] = cutImage(cutbmp, _CutX, _CutY, x, y);
            }
            Bitmap Img = new Bitmap(_ImgWidth, _ImgHeight);      //创建一张空白图片
            Graphics g = Graphics.FromImage(Img);   //从空白图片创建一个Graphics
            for (int i = 0; i < 20; i++)
            {
                //把图片指定坐标位置并画到空白图片上面
                g.DrawImage(bmp[i], new Point(i > 9 ? (i - 10) * _CutX : i * _CutX, i > 9 ? _CutY : 0));
            }
            g.Dispose();
            return Img;
        }

        //Bitmap转为base64编码的文本
        private string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch
            {
                //ImgToBase64String 转换失败\nException:" + ex.Message);
                return null;
            }
        }
        //base64编码的文本转为Bitmap
        private Bitmap Base64StringToImage(string txtBase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(txtBase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }
            catch
            {
                //Base64StringToImage 转换失败\nException：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 滑动特性
        /// </summary>
        private bool SlideFeature(string as_data)
        {
            if (string.IsNullOrEmpty(as_data))
                return false;
            string[] _datalist = as_data.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (_datalist.Length < 10)
                return false;
            //__array二维数组共两列 第一列为x轴坐标值 第二列为时间
            long[,] __array = new long[_datalist.Length, 2];
            #region 获取__array
            int row = 0;
            foreach (string str in _datalist)
            {
                string[] strlist = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (strlist.Length != 2)
                    return false;
                long __coor = 0, __date = 0;
                try { __coor = long.Parse(strlist[0]); __date = long.Parse(strlist[1]); }
                catch { return false; }
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                        __array[row, i] = __coor;
                    if (i == 1)
                        __array[row, i] = __date;
                }
                row++;
            }
            #endregion
            #region 计算速度 加速度 以及他们的标准差
            //速度 像素/每秒
            double[] __v = new double[_datalist.Length - 1];
            //加速度 像素/每2次方秒
            double[] __a = new double[_datalist.Length - 1];
            //总时间
            int __totaldate = 0;
            for (int i = 0; i < __v.Length; i++)
            {
                //时间差
                int __timeSpan = 0;
                if (__array[i + 1, 1] - __array[i, 1] == 0)
                    __timeSpan = 1;
                else
                    __timeSpan = (GetTime(__array[i + 1, 1].ToString()) - GetTime(__array[i, 1].ToString())).Milliseconds;
                __v[i] = (double)1000 * Math.Abs(__array[i + 1, 0] - __array[i, 0]) / __timeSpan;//有可能移过再一回来 这里只取正值
                __a[i] = (double)1000 * __v[i] / __timeSpan;
                __totaldate += __timeSpan;
            }
            //速度的计算公式：v=S/t
            //加速度计算公式：a=Δv/Δt
            //分析速度与加速度

            //平均速度
            double __mv = 0;
            double __sumv = 0;
            double __s2v = 0;//速度方差
            double __o2v = 0;//速度标准差
            foreach (double a in __v)
            {
                __sumv += a;
            }
            __mv = __sumv / __v.Length;
            __sumv = 0;
            for (int i = 0; i < __v.Length; i++)
            {
                __sumv += Math.Pow(__v[i] - __mv, 2);
            }
            __s2v = __sumv / __v.Length;
            __o2v = Math.Sqrt(__s2v);

            //平均加速度
            double __ma = 0;
            double __suma = 0;
            double __s2a = 0;//加速度方差
            double __o2a = 0;//加速度标准差
            foreach (double a in __a)
            {
                __suma += a;
            }
            __ma = __suma / __a.Length;
            __suma = 0;
            for (int i = 0; i < __a.Length; i++)
            {
                __suma += Math.Pow(__a[i] - __ma, 2);
            }
            __s2a = __suma / __v.Length;
            __o2a = Math.Sqrt(__s2a);

            double threeEqual = __a.Length / 3;
            //将加速度数组分成三等分 求每一份的加速度
            double __ma1 = 0, __ma2 = 0, __ma3 = 0;
            for (int i = 0; i < __a.Length; i++)
            {
                if (i > threeEqual * 2)
                    __ma3 += __a[i];
                else if (i > threeEqual && i < threeEqual * 2)
                    __ma2 += __a[i];
                else
                    __ma1 += __a[i];
            }
            __ma1 = __ma1 / threeEqual;
            __ma2 = __ma2 / threeEqual;
            __ma3 = __ma3 / threeEqual;
            //将速度数组分成三等分 求每一份的速度
            threeEqual = __v.Length / 3;
            double __mv1 = 0, __mv2 = 0, __mv3 = 0;
            for (int i = 0; i < __v.Length; i++)
            {
                if (i > threeEqual * 2)
                    __mv3 += __v[i];
                else if (i > threeEqual && i < threeEqual * 2)
                    __mv2 += __v[i];
                else
                    __mv1 += __v[i];
            }
            __mv1 = __mv1 / threeEqual;
            __mv2 = __mv2 / threeEqual;
            __mv3 = __mv3 / threeEqual;
            #endregion
            return true;
        }
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime now = dtStart.Add(toNow);
            return now;
        }
    }
}

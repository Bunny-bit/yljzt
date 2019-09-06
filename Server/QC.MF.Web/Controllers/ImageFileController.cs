using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp;
using Abp.Auditing;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using QC.MF.Authorization.Users;
//using QC.MF.Friendships;
using QC.MF.IO;
using QC.MF.Storage;
using Abp.IO;
using QC.MF.Configuration;
using System.Linq;
using Abp.Configuration;
using DataExporting.Net.MimeTypes;
using System.Collections;
using System.Drawing.Drawing2D;
using Abp.Web.Mvc.Controllers;
using System.Text.RegularExpressions;

namespace QC.MF.Web.Controllers
{
    public class ImageFileController : AbpController
    {
        /// <summary>
        /// 允许最大的文件长度
        /// </summary>
        private long MaxContentLength = 5048576;//5M  
        /// <summary>
        /// 允许上传的文件格式
        /// </summary>
        private List<ImageFormat> AcceptedFormats = new List<ImageFormat>
        {
            ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif
        };

       // private readonly UserManager _userManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IAppFolders _appFolders;
        private readonly ISettingManager _settingManager;

        public ImageFileController(
            UserManager userManager,
            IBinaryObjectManager binaryObjectManager,
            IAppFolders appFolders,
            ISettingManager settingManager
            )
        {
            //_userManager = userManager;
            _binaryObjectManager = binaryObjectManager;
            _appFolders = appFolders;
            _settingManager = settingManager;
        }

        private string GetImagesFolder()
        {
            var path = _appFolders.ImagesFolder;
            var settingPath = _settingManager.GetSettingValue(AppSettingNames.System.ImageUploadPath);
            if (!settingPath.IsNullOrWhiteSpace())
            {
                path = Server.MapPath(settingPath);
            }
            return path;
        }

        private string GetFilePath(ref string fileName)
        {
            var date = DateTime.Now;
            var userId = AbpSession.GetUserId();
            fileName = userId + "/" + date.Year + "/" + date.Month + "/" + date.Day + "/" + fileName;
            var filePath = GetImagesFolder() + "/" + fileName;
            return filePath;
        }

        [AbpMvcAuthorize]
        public JsonResult UploadPicture()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException(L("没有上传文件。"));
                }

                var file = Request.Files[0];

                if (file.ContentLength > MaxContentLength) 
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", $"{MaxContentLength/ 1024m / 1024}MB"));
                }

                //Check file type & format
                var fileImage = Image.FromStream(file.InputStream);
                if (!AcceptedFormats.Contains(fileImage.RawFormat))
                {
                    throw new ApplicationException($"上传的文件不是支持格式的图片（支持{string.Join("、",AcceptedFormats)}） !");
                }

                var fileName = file.FileName;
                var filePath = GetFilePath(ref fileName);
                //Delete old temp profile pictures
                FileHelper.DeleteIfExists(filePath);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                //Save new picture
                file.SaveAs(filePath);

                using (var bmpImage = new Bitmap(filePath))
                {
                    return Json(new AjaxResponse(new { fileName = fileName, width = bmpImage.Width, height = bmpImage.Height, url = $"\\ImageFile\\GetPictureHvtThumbnailByPath?fileName={fileName}" }));
                }
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        private FileResult GetDefaultProfilePicture()
        {
            return File(Server.MapPath("~/Common/Images/default-profile-picture.png"), MimeTypeNames.ImagePng);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="w">目标宽度</param>
        /// <param name="h">目标高度</param>
        /// <param name="cm">裁剪方式</param>
        /// <param name="x">自定义裁剪方式时，x坐标</param> 
        /// <param name="y">自定义裁剪方式时，y坐标</param> 
        /// <param name="rf">旋转和翻转</param> 
        /// <returns></returns>
        public async Task<FileResult> GetPictureHvtThumbnailByPath(string fileName, int? w = null, int? h = null, CuttingMethod cm = CuttingMethod.LeftTop, int? x = null, int? y = null, RotateFlipType? rf = null)
        {
            var path = Path.Combine(GetImagesFolder(), fileName);
            if (!System.IO.File.Exists(path))
            {
                return null;
            }


            if (!w.HasValue && !h.HasValue && !x.HasValue && !y.HasValue && !rf.HasValue)
            {
                return File(path, MimeTypeNames.ImageJpeg);
            }

            var pathHvt = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + $"_w{w}_h{h}_cm{cm}_x{x}_y{y}_rf{rf}{Path.GetExtension(path)}");
            //  检查是否存在，存在就读
            if (System.IO.File.Exists(pathHvt))
            {
                return File(pathHvt, MimeTypeNames.ImageJpeg);
            }

            //  不存在就重新生成（缩小、裁剪）
            Image image = new Bitmap(path);
            if (w.HasValue && !h.HasValue)
            {
                h = image.Height * w.Value / image.Width;
            }
            if (h.HasValue && !w.HasValue)
            {
                w = image.Width * h.Value / image.Height;
            }
            var hvtImg =
                    (w.HasValue && h.HasValue)
                    ? ImageManager.GetHvtThumbnail(image, w.Value, h.Value, cm, x, y)
                    : image;// 没有设宽高，就取原图。

            if (rf.HasValue)
            {
                hvtImg = ImageManager.RotateFlip(hvtImg, rf.Value);
            }

            MemoryStream ms = new MemoryStream();
            hvtImg.Save(ms, ImageFormat.Jpeg);
            var fileData = ms.ToArray();

            //  保存缓存
            SaveImage(fileData, pathHvt);

            return File(fileData, MimeTypeNames.ImageJpeg);
        }
        private void SaveImage(byte[] data, string fileName)
        {
            var file = System.IO.File.Create(fileName);
            file.Write(data, 0, data.Length);
            file.Close();
            file.Dispose();
        }

        public async Task<string[]> GetImageList()
        {
            String[] SearchExtensions =new[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
            var buildingList = new List<String>();
            var filePath = GetImagesFolder() + "/" + AbpSession.GetUserId();
            var localPath = Server.MapPath(filePath);
            buildingList.AddRange(Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
                .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()))
                .Where(x => !new Regex(@".*_w.*_h.*_cm.*_x.*_y.*_rf.*").IsMatch(Path.GetFileNameWithoutExtension(x)))
                .Select(x => filePath + x.Substring(localPath.Length).Replace("\\", "/")));
            int Total = buildingList.Count;

            var Start = String.IsNullOrEmpty(Request["start"]) ? 0 : Convert.ToInt32(Request["start"]);
            var Size = String.IsNullOrEmpty(Request["size"]) ? 10 : Convert.ToInt32(Request["size"]);
            var FileList = buildingList.OrderByDescending(x => x).Skip(Start).Take(Size).ToArray();
            return FileList;
        }

    }

    public class ImageManager
    {
        /// <summary> 
        /// 为图片生成缩略图 
        /// </summary> 
        /// <param name="phyPath">原图片的路径</param> 
        /// <param name="width">缩略图宽</param> 
        /// <param name="height">缩略图高</param> 
        /// <param name="cuttingMethod">裁剪方式</param> 
        /// <param name="x">自定义裁剪方式时，x坐标</param> 
        /// <param name="y">自定义裁剪方式时，y坐标</param> 
        /// <returns></returns> 
        public static Image GetHvtThumbnail(Image image, int width, int height, CuttingMethod cuttingMethod = CuttingMethod.LeftTop, int? x = null, int? y = null)
        {
            var img = Shrink(image, width, height);
            var img2 = Cutting(img, width, height, cuttingMethod, x, y);
            return img2;
        }
        /// <summary> 
        /// 为图片生成缩略图 
        /// </summary> 
        /// <param name="phyPath">原图片的路径</param> 
        /// <param name="width">缩略图宽</param> 
        /// <param name="height">缩略图高</param> 
        /// <param name="cuttingMethod">裁剪方式</param> 
        /// <param name="x">自定义裁剪方式时，x坐标</param> 
        /// <param name="y">自定义裁剪方式时，y坐标</param> 
        /// <returns></returns> 
        private static Image _GetHvtThumbnail(Image image, int width, int height, CuttingMethod cuttingMethod = CuttingMethod.LeftTop, int? x = null, int? y = null)
        {
            Bitmap m_hovertreeBmp = new Bitmap(width, height);
            //从Bitmap创建一个System.Drawing.Graphics 
            Graphics m_HvtGr = Graphics.FromImage(m_hovertreeBmp);
            //设置  
            m_HvtGr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //下面这个也设成高质量 
            m_HvtGr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //下面这个设成High 
            m_HvtGr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            int m_width, m_height;
            if (image.Width * height > image.Height * width)
            {
                m_height = image.Height;
                m_width = (image.Height * width) / height;
            }
            else
            {
                m_width = image.Width;
                m_height = (image.Width * height) / width;
            }
            //把原始图像绘制成上面所设置宽高的缩小图 
            Rectangle rectDestination = new Rectangle(0, 0, width, height);

            m_HvtGr.DrawImage(image, rectDestination, 0, 0, m_width, m_height, GraphicsUnit.Pixel);

            return m_hovertreeBmp;
        }
        /// <summary>
        /// 先缩小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private static Image Shrink(Image image, int width, int height)
        {
            int m_width, m_height;
            if (image.Width * height > image.Height * width)
            {
                m_height = height;
                m_width = (int)(m_height * (image.Width / (decimal)image.Height));
            }
            else
            {
                m_width = width;
                m_height = (int)(m_width / (image.Width / (decimal)image.Height));
            }
            var _image = _GetHvtThumbnail(image, m_width, m_height);
            return _image;
        }
        /// <summary>
        /// 再裁剪
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private static Image Cutting(Image image, int width, int height, CuttingMethod cuttingMethod = CuttingMethod.LeftTop, int? x = null, int? y = null)
        {
            Bitmap m_hovertreeBmp = new Bitmap(width, height);
            //从Bitmap创建一个System.Drawing.Graphics 
            Graphics m_HvtGr = Graphics.FromImage(m_hovertreeBmp);
            //设置  
            m_HvtGr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //下面这个也设成高质量 
            m_HvtGr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //下面这个设成High 
            m_HvtGr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            int m_width, m_height;
            if (image.Width * height > image.Height * width)
            {
                m_height = image.Height;
                m_width = (image.Height * width) / height;
            }
            else
            {
                m_width = image.Width;
                m_height = (image.Width * height) / width;
            }
            //原始图像需要裁剪的区域 
            Rectangle srcDestination = GetRectangle(cuttingMethod, image.Width, image.Height, width, height, x, y);

            m_HvtGr.DrawImage(image, new Rectangle(0, 0, width, height), srcDestination, GraphicsUnit.Pixel);

            return m_hovertreeBmp;
        }
        private static Rectangle GetRectangle(CuttingMethod cuttingMethod, int m_width, int m_height, int width, int height, int? x, int? y)
        {
            int _x = 0;
            int _y = 0;
            switch (cuttingMethod)
            {
                case CuttingMethod.LeftTop:
                    _x = 0;
                    _y = 0;
                    break;
                case CuttingMethod.LeftBottom:
                    _x = 0;
                    _y = m_height - height;
                    break;
                case CuttingMethod.Center:
                    _x = (m_width - width) / 2;
                    _y = (m_height - height) / 2;
                    break;
                case CuttingMethod.Customize:
                    if (!x.HasValue || !y.HasValue) { throw new UserFriendlyException("自定义裁剪模式必须设置裁剪坐标。"); }
                    _x = x.Value;
                    _y = y.Value;
                    break;
            }
            if (_x + width > m_width) { _x = m_width - width; }
            if (_y + height > m_height) { _y = m_height - height; }
            _x = _x < 0 ? 0 : _x;
            _y = _y < 0 ? 0 : _y;
            //_x = -_x;
            //_y = -_y;
            return new Rectangle(_x, _y, width, height);
        }

        /// <summary>
        /// 将图片进行翻转处理
        /// </summary>
        /// <param name="img">原始图片</param>
        /// <returns>经过翻转后的图片</returns>
        public static Bitmap RotateFlip(Image img, RotateFlipType rf)
        {
            int width = img.Width;
            int height = img.Height;

            Bitmap m_hovertreeBmp = new Bitmap(width, height);
            //从Bitmap创建一个System.Drawing.Graphics 
            Graphics m_HvtGr = Graphics.FromImage(m_hovertreeBmp);
            m_HvtGr.DrawImage(img, new PointF(0, 0));
            m_hovertreeBmp.RotateFlip(rf);
            return m_hovertreeBmp;//返回经过翻转后的图片
        }
        #region 图片水印
        /// <summary>
        /// 图片水印处理方法
        /// </summary>
        /// <param name="path">需要加载水印的图片</param>
        /// <param name="waterpath">水印图片</param>
        /// <param name="location">水印位置（传送正确的代码）</param>
        public static Image ImageWatermark(Image img, Image waterimg, string location)
        {
            DateTime time = DateTime.Now;
            string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
            Graphics g = Graphics.FromImage(img);
            ArrayList loca = GetLocation(location, img, waterimg);
            g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
            waterimg.Dispose();
            g.Dispose();
            return img;
        }

        /// <summary>
        /// 图片水印位置处理方法
        /// </summary>
        /// <param name="location">水印位置</param>
        /// <param name="img">需要添加水印的图片</param>
        /// <param name="waterimg">水印图片</param>
        private static ArrayList GetLocation(string location, Image img, Image waterimg)
        {
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;

            if (location == "LT")
            {
                x = 10;
                y = 10;
            }
            else if (location == "T")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else if (location == "RT")
            {
                x = img.Width - waterimg.Width;
                y = 10;
            }
            else if (location == "LC")
            {
                x = 10;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - waterimg.Width;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "LB")
            {
                x = 10;
                y = img.Height - waterimg.Height;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else
            {
                x = img.Width - waterimg.Width;
                y = img.Height - waterimg.Height;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
        }
        #endregion

        #region 文字水印
        /// <summary>
        /// 文字水印处理方法
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="size">字体大小</param>
        /// <param name="letter">水印文字</param>
        /// <param name="color">颜色</param>
        /// <param name="location">水印位置</param>
        public static Image LetterWatermark(Image img, int size, string letter, Color color, string location)
        {
            DateTime time = DateTime.Now;
            string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
            Graphics gs = Graphics.FromImage(img);
            ArrayList loca = GetLocation(location, img, size, letter.Length);
            Font font = new Font("宋体", size);
            Brush br = new SolidBrush(color);
            gs.DrawString(letter, font, br, float.Parse(loca[0].ToString()), float.Parse(loca[1].ToString()));
            gs.Dispose();
            return img;
        }

        /// <summary>
        /// 文字水印位置的方法
        /// </summary>
        /// <param name="location">位置代码</param>
        /// <param name="img">图片对象</param>
        /// <param name="width">宽(当水印类型为文字时,传过来的就是字体的大小)</param>
        /// <param name="height">高(当水印类型为文字时,传过来的就是字符的长度)</param>
        private static ArrayList GetLocation(string location, Image img, int width, int height)
        {
            #region

            ArrayList loca = new ArrayList();  //定义数组存储位置
            float x = 10;
            float y = 10;
            switch (location)
            {
                case "LT":
                    loca.Add(x);
                    loca.Add(y);
                    break;
                case "T":
                    x = img.Width / 2 - (width * height) / 2;
                    loca.Add(x);
                    loca.Add(y);
                    break;
                case "RT":
                    x = img.Width - width * height;
                    break;
                case "LC":
                    y = img.Height / 2;
                    break;
                case "C":
                    x = img.Width / 2 - (width * height) / 2;
                    y = img.Height / 2;
                    break;
                case "RC":
                    x = img.Width - height;
                    y = img.Height / 2;
                    break;
                case "LB":
                    y = img.Height - width - 5;
                    break;
                case "B":
                    x = img.Width / 2 - (width * height) / 2;
                    y = img.Height - width - 5;
                    break;
                default:
                    x = img.Width - width * height;
                    y = img.Height - width - 5;
                    break;
            }

            loca.Add(x);
            loca.Add(y);
            return loca;

            #endregion
        }
        #endregion

        #region 调整光暗
        /// <summary>
        /// 调整光暗
        /// </summary>
        /// <param name="mybm">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        /// <param name="val">增加或减少的光暗值</param>
        public Bitmap LDPic(Bitmap mybm, int width, int height, int val)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录经过处理后的图片对象
            int x, y, resultR, resultG, resultB;//x、y是循环次数，后面三个是记录红绿蓝三个值的
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值
                    resultR = pixel.R + val;//检查红色值会不会超出[0, 255]
                    resultG = pixel.G + val;//检查绿色值会不会超出[0, 255]
                    resultB = pixel.B + val;//检查蓝色值会不会超出[0, 255]
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//绘图
                }
            }
            return bm;
        }
        #endregion

        #region 反色处理
        /// <summary>
        /// 反色处理
        /// </summary>
        /// <param name="mybm">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public Bitmap RePic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录处理后的图片的对象
            int x, y, resultR, resultG, resultB;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值
                    resultR = 255 - pixel.R;//反红
                    resultG = 255 - pixel.G;//反绿
                    resultB = 255 - pixel.B;//反蓝
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//绘图
                }
            }
            return bm;
        }
        #endregion

        #region 浮雕处理
        /// <summary>
        /// 浮雕处理
        /// </summary>
        /// <param name="oldBitmap">原始图片</param>
        /// <param name="Width">原始图片的长度</param>
        /// <param name="Height">原始图片的高度</param>
        public Bitmap FD(Bitmap oldBitmap, int Width, int Height)
        {
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color color1, color2;
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    color1 = oldBitmap.GetPixel(x, y);
                    color2 = oldBitmap.GetPixel(x + 1, y + 1);
                    r = Math.Abs(color1.R - color2.R + 128);
                    g = Math.Abs(color1.G - color2.G + 128);
                    b = Math.Abs(color1.B - color2.B + 128);
                    if (r > 255) r = 255;
                    if (r < 0) r = 0;
                    if (g > 255) g = 255;
                    if (g < 0) g = 0;
                    if (b > 255) b = 255;
                    if (b < 0) b = 0;
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }
        #endregion

        #region 拉伸图片
        /// <summary>
        /// 拉伸图片
        /// </summary>
        /// <param name="bmp">原始图片</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap bap = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(bap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bap.Width, bap.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return bap;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 滤色处理
        /// <summary>
        /// 滤色处理
        /// </summary>
        /// <param name="mybm">原始图片</param>
        /// <param name="width">原始图片的长度</param>
        /// <param name="height">原始图片的高度</param>
        public Bitmap FilPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录滤色效果的图片对象
            int x, y;
            Color pixel;

            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值
                    bm.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B));//绘图
                }
            }
            return bm;
        }
        #endregion


        #region 图片灰度化
        public Color Gray(Color c)
        {
            int rgb = Convert.ToInt32((double)(((0.3 * c.R) + (0.59 * c.G)) + (0.11 * c.B)));
            return Color.FromArgb(rgb, rgb, rgb);
        }
        #endregion

        #region 转换为黑白图片
        /// <summary>
        /// 转换为黑白图片
        /// </summary>
        /// <param name="mybt">要进行处理的图片</param>
        /// <param name="width">图片的长度</param>
        /// <param name="height">图片的高度</param>
        public Bitmap BWPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, result; //x,y是循环次数，result是记录处理后的像素值
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值
                    result = (pixel.R + pixel.G + pixel.B) / 3;//取红绿蓝三色的平均值
                    bm.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return bm;
        }
        #endregion

        #region 获取图片中的各帧
        /// <summary>
        /// 获取图片中的各帧
        /// </summary>
        /// <param name="pPath">图片路径</param>
        /// <param name="pSavePath">保存路径</param>
        public void GetFrames(string pPath, string pSavedPath)
        {
            Image gif = Image.FromFile(pPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int count = gif.GetFrameCount(fd); //获取帧数(gif图片可能包含多帧，其它格式图片一般仅一帧)
            for (int i = 0; i < count; i++)    //以Jpeg格式保存各帧
            {
                gif.SelectActiveFrame(fd, i);
                gif.Save(pSavedPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }
        #endregion
    }
    public enum CuttingMethod
    {
        /// <summary>
        /// 从左上角开始裁剪
        /// </summary>
        LeftTop = 1,
        /// <summary>
        /// 从左下角开始裁剪
        /// </summary>
        LeftBottom,
        /// <summary>
        /// 中间裁剪
        /// </summary>
        Center,
        /// <summary>
        /// 自定义坐标裁剪
        /// </summary>
        Customize,
    }
}

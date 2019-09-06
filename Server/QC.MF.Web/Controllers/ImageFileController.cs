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
        /// ���������ļ�����
        /// </summary>
        private long MaxContentLength = 5048576;//5M  
        /// <summary>
        /// �����ϴ����ļ���ʽ
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
                    throw new UserFriendlyException(L("û���ϴ��ļ���"));
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
                    throw new ApplicationException($"�ϴ����ļ�����֧�ָ�ʽ��ͼƬ��֧��{string.Join("��",AcceptedFormats)}�� !");
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
        /// ��ȡͼƬ
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="w">Ŀ����</param>
        /// <param name="h">Ŀ��߶�</param>
        /// <param name="cm">�ü���ʽ</param>
        /// <param name="x">�Զ���ü���ʽʱ��x����</param> 
        /// <param name="y">�Զ���ü���ʽʱ��y����</param> 
        /// <param name="rf">��ת�ͷ�ת</param> 
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
            //  ����Ƿ���ڣ����ھͶ�
            if (System.IO.File.Exists(pathHvt))
            {
                return File(pathHvt, MimeTypeNames.ImageJpeg);
            }

            //  �����ھ��������ɣ���С���ü���
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
                    : image;// û�����ߣ���ȡԭͼ��

            if (rf.HasValue)
            {
                hvtImg = ImageManager.RotateFlip(hvtImg, rf.Value);
            }

            MemoryStream ms = new MemoryStream();
            hvtImg.Save(ms, ImageFormat.Jpeg);
            var fileData = ms.ToArray();

            //  ���滺��
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
        /// ΪͼƬ��������ͼ 
        /// </summary> 
        /// <param name="phyPath">ԭͼƬ��·��</param> 
        /// <param name="width">����ͼ��</param> 
        /// <param name="height">����ͼ��</param> 
        /// <param name="cuttingMethod">�ü���ʽ</param> 
        /// <param name="x">�Զ���ü���ʽʱ��x����</param> 
        /// <param name="y">�Զ���ü���ʽʱ��y����</param> 
        /// <returns></returns> 
        public static Image GetHvtThumbnail(Image image, int width, int height, CuttingMethod cuttingMethod = CuttingMethod.LeftTop, int? x = null, int? y = null)
        {
            var img = Shrink(image, width, height);
            var img2 = Cutting(img, width, height, cuttingMethod, x, y);
            return img2;
        }
        /// <summary> 
        /// ΪͼƬ��������ͼ 
        /// </summary> 
        /// <param name="phyPath">ԭͼƬ��·��</param> 
        /// <param name="width">����ͼ��</param> 
        /// <param name="height">����ͼ��</param> 
        /// <param name="cuttingMethod">�ü���ʽ</param> 
        /// <param name="x">�Զ���ü���ʽʱ��x����</param> 
        /// <param name="y">�Զ���ü���ʽʱ��y����</param> 
        /// <returns></returns> 
        private static Image _GetHvtThumbnail(Image image, int width, int height, CuttingMethod cuttingMethod = CuttingMethod.LeftTop, int? x = null, int? y = null)
        {
            Bitmap m_hovertreeBmp = new Bitmap(width, height);
            //��Bitmap����һ��System.Drawing.Graphics 
            Graphics m_HvtGr = Graphics.FromImage(m_hovertreeBmp);
            //����  
            m_HvtGr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //�������Ҳ��ɸ����� 
            m_HvtGr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //����������High 
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
            //��ԭʼͼ����Ƴ����������ÿ�ߵ���Сͼ 
            Rectangle rectDestination = new Rectangle(0, 0, width, height);

            m_HvtGr.DrawImage(image, rectDestination, 0, 0, m_width, m_height, GraphicsUnit.Pixel);

            return m_hovertreeBmp;
        }
        /// <summary>
        /// ����С
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
        /// �ٲü�
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private static Image Cutting(Image image, int width, int height, CuttingMethod cuttingMethod = CuttingMethod.LeftTop, int? x = null, int? y = null)
        {
            Bitmap m_hovertreeBmp = new Bitmap(width, height);
            //��Bitmap����һ��System.Drawing.Graphics 
            Graphics m_HvtGr = Graphics.FromImage(m_hovertreeBmp);
            //����  
            m_HvtGr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //�������Ҳ��ɸ����� 
            m_HvtGr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //����������High 
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
            //ԭʼͼ����Ҫ�ü������� 
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
                    if (!x.HasValue || !y.HasValue) { throw new UserFriendlyException("�Զ���ü�ģʽ�������òü����ꡣ"); }
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
        /// ��ͼƬ���з�ת����
        /// </summary>
        /// <param name="img">ԭʼͼƬ</param>
        /// <returns>������ת���ͼƬ</returns>
        public static Bitmap RotateFlip(Image img, RotateFlipType rf)
        {
            int width = img.Width;
            int height = img.Height;

            Bitmap m_hovertreeBmp = new Bitmap(width, height);
            //��Bitmap����һ��System.Drawing.Graphics 
            Graphics m_HvtGr = Graphics.FromImage(m_hovertreeBmp);
            m_HvtGr.DrawImage(img, new PointF(0, 0));
            m_hovertreeBmp.RotateFlip(rf);
            return m_hovertreeBmp;//���ؾ�����ת���ͼƬ
        }
        #region ͼƬˮӡ
        /// <summary>
        /// ͼƬˮӡ������
        /// </summary>
        /// <param name="path">��Ҫ����ˮӡ��ͼƬ</param>
        /// <param name="waterpath">ˮӡͼƬ</param>
        /// <param name="location">ˮӡλ�ã�������ȷ�Ĵ��룩</param>
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
        /// ͼƬˮӡλ�ô�����
        /// </summary>
        /// <param name="location">ˮӡλ��</param>
        /// <param name="img">��Ҫ���ˮӡ��ͼƬ</param>
        /// <param name="waterimg">ˮӡͼƬ</param>
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

        #region ����ˮӡ
        /// <summary>
        /// ����ˮӡ������
        /// </summary>
        /// <param name="img">ͼƬ</param>
        /// <param name="size">�����С</param>
        /// <param name="letter">ˮӡ����</param>
        /// <param name="color">��ɫ</param>
        /// <param name="location">ˮӡλ��</param>
        public static Image LetterWatermark(Image img, int size, string letter, Color color, string location)
        {
            DateTime time = DateTime.Now;
            string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
            Graphics gs = Graphics.FromImage(img);
            ArrayList loca = GetLocation(location, img, size, letter.Length);
            Font font = new Font("����", size);
            Brush br = new SolidBrush(color);
            gs.DrawString(letter, font, br, float.Parse(loca[0].ToString()), float.Parse(loca[1].ToString()));
            gs.Dispose();
            return img;
        }

        /// <summary>
        /// ����ˮӡλ�õķ���
        /// </summary>
        /// <param name="location">λ�ô���</param>
        /// <param name="img">ͼƬ����</param>
        /// <param name="width">��(��ˮӡ����Ϊ����ʱ,�������ľ�������Ĵ�С)</param>
        /// <param name="height">��(��ˮӡ����Ϊ����ʱ,�������ľ����ַ��ĳ���)</param>
        private static ArrayList GetLocation(string location, Image img, int width, int height)
        {
            #region

            ArrayList loca = new ArrayList();  //��������洢λ��
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

        #region �����ⰵ
        /// <summary>
        /// �����ⰵ
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        /// <param name="val">���ӻ���ٵĹⰵֵ</param>
        public Bitmap LDPic(Bitmap mybm, int width, int height, int val)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼����������ͼƬ����
            int x, y, resultR, resultG, resultB;//x��y��ѭ�����������������Ǽ�¼����������ֵ��
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���ص�ֵ
                    resultR = pixel.R + val;//����ɫֵ�᲻�ᳬ��[0, 255]
                    resultG = pixel.G + val;//�����ɫֵ�᲻�ᳬ��[0, 255]
                    resultB = pixel.B + val;//�����ɫֵ�᲻�ᳬ��[0, 255]
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//��ͼ
                }
            }
            return bm;
        }
        #endregion

        #region ��ɫ����
        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public Bitmap RePic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼������ͼƬ�Ķ���
            int x, y, resultR, resultG, resultB;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    resultR = 255 - pixel.R;//����
                    resultG = 255 - pixel.G;//����
                    resultB = 255 - pixel.B;//����
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//��ͼ
                }
            }
            return bm;
        }
        #endregion

        #region ������
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="oldBitmap">ԭʼͼƬ</param>
        /// <param name="Width">ԭʼͼƬ�ĳ���</param>
        /// <param name="Height">ԭʼͼƬ�ĸ߶�</param>
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

        #region ����ͼƬ
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="bmp">ԭʼͼƬ</param>
        /// <param name="newW">�µĿ��</param>
        /// <param name="newH">�µĸ߶�</param>
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

        #region ��ɫ����
        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public Bitmap FilPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼��ɫЧ����ͼƬ����
            int x, y;
            Color pixel;

            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    bm.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B));//��ͼ
                }
            }
            return bm;
        }
        #endregion


        #region ͼƬ�ҶȻ�
        public Color Gray(Color c)
        {
            int rgb = Convert.ToInt32((double)(((0.3 * c.R) + (0.59 * c.G)) + (0.11 * c.B)));
            return Color.FromArgb(rgb, rgb, rgb);
        }
        #endregion

        #region ת��Ϊ�ڰ�ͼƬ
        /// <summary>
        /// ת��Ϊ�ڰ�ͼƬ
        /// </summary>
        /// <param name="mybt">Ҫ���д����ͼƬ</param>
        /// <param name="width">ͼƬ�ĳ���</param>
        /// <param name="height">ͼƬ�ĸ߶�</param>
        public Bitmap BWPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, result; //x,y��ѭ��������result�Ǽ�¼����������ֵ
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    result = (pixel.R + pixel.G + pixel.B) / 3;//ȡ��������ɫ��ƽ��ֵ
                    bm.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return bm;
        }
        #endregion

        #region ��ȡͼƬ�еĸ�֡
        /// <summary>
        /// ��ȡͼƬ�еĸ�֡
        /// </summary>
        /// <param name="pPath">ͼƬ·��</param>
        /// <param name="pSavePath">����·��</param>
        public void GetFrames(string pPath, string pSavedPath)
        {
            Image gif = Image.FromFile(pPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int count = gif.GetFrameCount(fd); //��ȡ֡��(gifͼƬ���ܰ�����֡��������ʽͼƬһ���һ֡)
            for (int i = 0; i < count; i++)    //��Jpeg��ʽ�����֡
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
        /// �����Ͻǿ�ʼ�ü�
        /// </summary>
        LeftTop = 1,
        /// <summary>
        /// �����½ǿ�ʼ�ü�
        /// </summary>
        LeftBottom,
        /// <summary>
        /// �м�ü�
        /// </summary>
        Center,
        /// <summary>
        /// �Զ�������ü�
        /// </summary>
        Customize,
    }
}

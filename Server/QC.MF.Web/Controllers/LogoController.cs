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
using QC.MF.IO;
using QC.MF.Storage;
using Abp.IO;
using DataExporting.Net.MimeTypes;

namespace QC.MF.Web.Controllers
{
    public class LogoController : MFControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IAppFolders _appFolders;

        public LogoController(
            UserManager userManager,
            IBinaryObjectManager binaryObjectManager,
            IAppFolders appFolders
            )
        {
            _userManager = userManager;
            _binaryObjectManager = binaryObjectManager;
            _appFolders = appFolders;
        }

        [DisableAuditing]
        public FileResult GetLogoPicture()
        {
            return File(Server.MapPath("~/Common/logo.png"), MimeTypeNames.ImagePng);
        }

        [AbpMvcAuthorize]
        public JsonResult UploadLogoPicture()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException("未找到文件");
                }

                var file = Request.Files[0];

                if (file.ContentLength > 30 * 1024)
                {
                    throw new UserFriendlyException("logo文件不能大于30kb");
                }

                //Check file type & format
                var fileImage = Image.FromStream(file.InputStream);
                var acceptedFormats = new List<ImageFormat>
                {
                    ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif
                };

                if (!acceptedFormats.Contains(fileImage.RawFormat))
                {
                    throw new ApplicationException("未能识别该类型的文件");
                }
                var path = Server.MapPath("~/Common/logo.png");
                FileHelper.DeleteIfExists(path);

                file.SaveAs(path);

                using (var bmpImage = new Bitmap(path))
                {
                    return Json(new AjaxResponse(new { fileName = "/Common/logo.png", width = bmpImage.Width, height = bmpImage.Height }));
                }
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}

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
using DataExporting.Net.MimeTypes;

namespace QC.MF.Web.Controllers
{
    public class ProfileController : MFControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IAppFolders _appFolders;
        //private readonly IFriendshipManager _friendshipManager;

        public ProfileController(
            UserManager userManager,
            IBinaryObjectManager binaryObjectManager,
            IAppFolders appFolders
            //,
            //IFriendshipManager friendshipManager
            )
        {
            _userManager = userManager;
            _binaryObjectManager = binaryObjectManager;
            _appFolders = appFolders;
            //_friendshipManager = friendshipManager;
        }

        [DisableAuditing]
        public async Task<FileResult> GetProfilePicture()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());
            if (user.ProfilePictureId == null)
            {
                return GetDefaultProfilePicture();
            }

            return await GetProfilePictureById(user.ProfilePictureId.Value);
        }

        [DisableAuditing]
        public async Task<FileResult> GetProfilePictureById(string id = "")
        {
            if (id.IsNullOrEmpty())
            {
                return GetDefaultProfilePicture();
            }

            return await GetProfilePictureById(Guid.Parse(id));
        }

        //[DisableAuditing]
        //[UnitOfWork]
        //public virtual async Task<FileResult> GetFriendProfilePictureById(long userId, int? tenantId, string id = "")
        //{
        //    if (id.IsNullOrEmpty() ||
        //        _friendshipManager.GetFriendshipOrNull(AbpSession.ToUserIdentifier(), new UserIdentifier(tenantId, userId)) == null)
        //    {
        //        return GetDefaultProfilePicture();
        //    }

        //    using (CurrentUnitOfWork.SetTenantId(tenantId))
        //    {
        //        return await GetProfilePictureById(Guid.Parse(id));
        //    }
        //}

        //TODO: DELETE not used action!!!
        [UnitOfWork]
        [AbpMvcAuthorize]
        public virtual async Task<JsonResult> ChangeProfilePicture()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                var file = Request.Files[0];

                if (file.ContentLength > 30720) //30KB.
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", "30KB"));
                }

                //Get user
                var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());

                //Delete old picture
                if (user.ProfilePictureId.HasValue)
                {
                    await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
                }

                //Save new picture
                var storedFile = new BinaryObject(AbpSession.TenantId, file.InputStream.GetAllBytes());
                await _binaryObjectManager.SaveAsync(storedFile);

                //Update new picture on the user
                user.ProfilePictureId = storedFile.Id;

                //Return success
                return Json(new AjaxResponse());
            }
            catch (UserFriendlyException ex)
            {
                //Return error message
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [AbpMvcAuthorize]
        public JsonResult UploadProfilePicture()
        {
            try
            {
                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                var file = Request.Files[0];

                if (file.ContentLength > 1048576) //1MB.
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
                }

                //Check file type & format
                var fileImage = Image.FromStream(file.InputStream);
                var acceptedFormats = new List<ImageFormat>
                {
                    ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif
                };

                if (!acceptedFormats.Contains(fileImage.RawFormat))
                {
                    throw new ApplicationException("Uploaded file is not an accepted image file !");
                }

                //Delete old temp profile pictures
                AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.TempFileDownloadFolder, "userProfileImage_" + AbpSession.GetUserId());

                //Save new picture
                var fileInfo = new FileInfo(file.FileName);
                var tempFileName = "userProfileImage_" + AbpSession.GetUserId() + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFileName);
                file.SaveAs(tempFilePath);

                using (var bmpImage = new Bitmap(tempFilePath))
                {
                    return Json(new AjaxResponse(new { fileName = tempFileName, width = bmpImage.Width, height = bmpImage.Height }));
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

        private async Task<FileResult> GetProfilePictureById(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return GetDefaultProfilePicture();
            }

            return File(file.Bytes, MimeTypeNames.ImageJpeg);
        }

        /// <summary> 
        /// 为图片生成缩略图 
        /// </summary> 
        /// <param name="phyPath">原图片的路径</param> 
        /// <param name="width">缩略图宽</param> 
        /// <param name="height">缩略图高</param> 
        /// <returns></returns> 
        private System.Drawing.Image GetHvtThumbnail(System.Drawing.Image image, int width, int height)
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
            //把原始图像绘制成上面所设置宽高的缩小图 
            Rectangle rectDestination = new Rectangle(0, 0, width, height);

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

            m_HvtGr.DrawImage(image, rectDestination, 0, 0, m_width, m_height, GraphicsUnit.Pixel);

            return m_hovertreeBmp;
        }
    }
}

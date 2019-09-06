using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QC.MF.WebFiles
{
    public class WebFileManager : IWebFileManager, ITransientDependency
    {
        public const string TempFileFolder = "~\\TempUploadFile";
        public const string UploadFile = "~\\UploadFile";
        private readonly IRepository<WebFile, Guid> _webFileRepository;
        public WebFileManager(IRepository<WebFile, Guid> webFileRepository)
        {
            _webFileRepository = webFileRepository;
        }


        public Task<WebFile> GetOrNullAsync(Guid id)
        {
            return _webFileRepository.FirstOrDefaultAsync(id);
        }
        public async Task<FileInfo> GetFileInfoOrNullAsync(Guid id)
        {
            var webFile = await _webFileRepository.FirstOrDefaultAsync(id);
            if (webFile != null)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(webFile.FilePath)))
                {
                    return new FileInfo(HttpContext.Current.Server.MapPath(webFile.FilePath));
                }
            }
            return null;
        }

        public async Task<Guid> UploadFileAsync()
        {
            var httpFile = HttpContext.Current.Request.Files?[0];
            if (httpFile == null)
            {
                throw new Abp.UI.UserFriendlyException("没找到文件");
            }

            var webFile = new WebFile();
            webFile.FileName = httpFile.FileName;
            webFile.TempFilePath = $"{TempFileFolder}\\{DateTime.Now.ToString("yyyyMMdd")}";
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(webFile.TempFilePath));
            webFile.TempFilePath = $"{ webFile.TempFilePath}\\{webFile.Id}";
            httpFile.SaveAs(HttpContext.Current.Server.MapPath(webFile.TempFilePath));
            await _webFileRepository.InsertAsync(webFile);
            return webFile.Id;
        }

        public async Task UserFileAsync(Guid id, Guid? oldId = null, string newPath = UploadFile)
        {
            if (id == oldId) { return; }
            var webFile = await _webFileRepository.FirstOrDefaultAsync(id);
            if (webFile == null || !File.Exists(HttpContext.Current.Server.MapPath(webFile.TempFilePath)))
            {
                throw new Abp.UI.UserFriendlyException("没找到文件");
            }
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(newPath));
            webFile.FilePath = $"{newPath}\\{ webFile.Id}";
            File.Move(HttpContext.Current.Server.MapPath(webFile.TempFilePath), HttpContext.Current.Server.MapPath(webFile.FilePath));
            webFile.TempFilePath = null;

            if (oldId != null) { await DeleteAsync(oldId.Value); }
        }

        public async Task DownloadFileAsync(Guid id)
        {
            WebFile webFile = await GetWebFile(id);

            var Response = HttpContext.Current.Response;
            Response.Clear();
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", webFile.FileName));
            Response.TransmitFile(HttpContext.Current.Server.MapPath(webFile.FilePath));
            Response.End();
        }

        private async Task<WebFile> GetWebFile(Guid id)
        {
            var webFile = await _webFileRepository.FirstOrDefaultAsync(id);
            if (webFile == null || !File.Exists(HttpContext.Current.Server.MapPath(webFile.FilePath)))
            {
                throw new Abp.UI.UserFriendlyException("没找到文件");
            }

            return webFile;
        }

        public async Task DeleteAsync(Guid id)
        {
            var webFile = await _webFileRepository.FirstOrDefaultAsync(id);
            if (webFile != null)
            {
                await DeleteAsync(webFile);
            }
        }

        private async Task DeleteAsync(WebFile webFile)
        {
            await _webFileRepository.DeleteAsync(webFile);
            if (File.Exists(HttpContext.Current.Server.MapPath(webFile.FilePath)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(webFile.FilePath));
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(webFile.TempFilePath)))
            {
                File.Delete(HttpContext.Current.Server.MapPath(webFile.TempFilePath));
            }
        }
    }
}

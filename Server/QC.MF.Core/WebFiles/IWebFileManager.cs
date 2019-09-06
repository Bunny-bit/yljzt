using System;
using System.IO;
using System.Threading.Tasks;

namespace QC.MF.WebFiles
{
    public interface IWebFileManager
    {
        Task DeleteAsync(Guid id);
        Task DownloadFileAsync(Guid id);
        Task<FileInfo> GetFileInfoOrNullAsync(Guid id);
        Task<WebFile> GetOrNullAsync(Guid id);
        Task<Guid> UploadFileAsync();
        Task UserFileAsync(Guid id, Guid? oldId = null, string newPath = "~\\UploadFile");
    }
}

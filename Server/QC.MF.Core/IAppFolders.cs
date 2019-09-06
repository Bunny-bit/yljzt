namespace QC.MF
{
    public interface IAppFolders
    {
        string TempFileDownloadFolder { get; }

        string SampleProfileImagesFolder { get; }

        string ImagesFolder { get; }

        string WebLogsFolder { get; set; }

        string DragVerificationImageFolder { get; set; }
    }
}

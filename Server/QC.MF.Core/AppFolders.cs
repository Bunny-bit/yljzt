using Abp.Dependency;

namespace QC.MF
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string TempFileDownloadFolder { get; set; }

        public string SampleProfileImagesFolder { get; set; }

        public string WebLogsFolder { get; set; }

        public string DragVerificationImageFolder { get; set; }

        public string ImagesFolder { get; set; }

    }
}

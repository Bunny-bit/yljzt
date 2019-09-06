using System.Web;
using Abp.Configuration;
using Newtonsoft.Json;
using QC.MF.Configuration;

namespace QC.MF.InterfaceExport
{
    /// <summary>
    /// 接口导出实现
    /// </summary>
    public class InterfaceExportAppService : MFAppServiceBase, IInterfaceExportAppService
    {
        private const string SwaggerUrl = "http://generator.swagger.io/api/gen/clients/typescript-fetch";
        private const string DownloadUrl = "http://generator.swagger.io/api/gen/download/";
        /// <inheritdoc />
        public string GetReactDownloadUrl()
        {
            var data = HttpRequester.Request(GetSwaggerApiUrl(), new HttpRequester.RequestOptions());
            var postData = $"{{\"spec\":{data}}}";
            var download = HttpRequester.Request(SwaggerUrl, 
                new HttpRequester.RequestOptions
                {
                    Method = HttpRequester.HttpMethod.Post,
                }, 
                postData);
            return DownloadUrl + JsonConvert.DeserializeObject<dynamic>(download).code;
        }

        private string GetSwaggerApiUrl()
        {
            return $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}:{HttpContext.Current.Request.Url.Port}/swagger/docs/v1";
        }
    }
}

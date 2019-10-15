using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using Abp.UI;

namespace QC.MF
{
    public class HttpRequester
    {
        /// <summary>
        /// HTTP请求方式枚举
        /// </summary>
        public enum HttpMethod { Post, Get };

        public class RequestOptions
        {
            public HttpMethod Method { get; set; }=HttpMethod.Get;
            public string Accept { get; set; } = "*/*";

            public string ContentType { get; set; } = "application/json; charset=utf-8";

            public string UserAgent { get; set; } =
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36";
        }

        /// <summary>
        /// 建立HTTP请求，返回请求结果
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="options">请求方式</param>
        /// <param name="body">请求参数</param>
        /// <param name="timeout">请求超时时间</param>
        /// <returns>返回请求结果</returns>
        public static string Request(string url, RequestOptions options, string body = "", int timeout = 60000)
        {
            string readStr = string.Empty;
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(url);
            http.Method = options.Method.ToString();
            http.Accept = options.Accept;
            http.ContentType = options.ContentType;
            http.UserAgent = options.UserAgent;
            Encoding encoder = Encoding.GetEncoding("UTF-8");

            if (options.Method == HttpMethod.Get)
            {
                if (body.Length > 1)
                {
                    url += body;
                }
            }
            if (options.Method == HttpMethod.Post)
            {
                if (body.Length > 1)
                {
                    byte[] buffer = encoder.GetBytes(body);
                    http.ContentLength = buffer.Length;
                    try
                    {
                        http.GetRequestStream().Write(buffer, 0, buffer.Length);
                    }
                    catch(Exception ex)
                    {
                        throw new UserFriendlyException(ex.Message);
                    }
                }
            }

            http.Timeout = timeout;

            HttpWebResponse httpRs; try
            {
                httpRs = (HttpWebResponse)http.GetResponse();
            }
            catch (WebException ex)
            {
                httpRs = (HttpWebResponse)ex.Response;
            }
            if (httpRs == null || (httpRs.StatusCode != HttpStatusCode.OK &&
                                   httpRs.StatusCode != HttpStatusCode.Created &&
                                   httpRs.StatusCode != HttpStatusCode.Accepted))
            {
                throw new UserFriendlyException("未能获取您需要的信息，请重试！");
            }
            using (Stream stream = httpRs.GetResponseStream())
            {
                using (StreamReader read = new StreamReader(stream, encoder))
                {
                    while (!read.EndOfStream)
                    {
                        readStr = readStr + read.ReadLine();
                    }
                    read.Close();
                }
                stream.Close();
            }
            httpRs.Close();
            http.Abort();
            return readStr;
        }
    }
}

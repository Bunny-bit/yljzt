using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Abp.UI;

namespace QC.MF
{
    public static class EncryptExtensions
    {
        #region 加密Key
        //URL传输参数加密Key这个key可以自己设置支持8位这个东西很重要的
        private const string QueryStringKey = "!^HF*KD:"; 

        #endregion

        #region 加密算法
        /// <summary>
        /// 加密算法
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string EncryptQueryString(this string queryString)
        {
            return Encrypt(queryString, QueryStringKey);
        }
        #endregion

        #region 解密算法
        /// <summary>
        /// 解密算法
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string DecryptQueryString(this string queryString)
        {
            return Decrypt(queryString, QueryStringKey);
        }
        #endregion


        public static string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //把字符串放到byte数组中

            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);

            des.Key = Encoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量
            des.IV = Encoding.ASCII.GetBytes(sKey); //原文使用ASCIIEncoding.ASCII方法的GetBytes方法
            MemoryStream ms = new MemoryStream(); //使得输入密码必须输入英文文本
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        public static string Decrypt(string pToDecrypt, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte) i;
                }

                des.Key = Encoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量，此值重要，不能修改
                des.IV = Encoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                throw new UserFriendlyException("加密串无效");
            }
        }

        public static bool ValidateString(string enString, string foString, int mode)
        {
            switch (mode)
            {
                default:
                    if (Decrypt(enString, QueryStringKey) == foString)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

            }
        }

        /// <summary>
        /// RSA 加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RsaEncrypt(string publickey, string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RsaDecrypt(string privatekey, string content)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(privatekey);
                var cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
                return Encoding.UTF8.GetString(cipherbytes);
            }
            catch (Exception )
            {
                return "error";
            }
        }


        #region MD5 加密字符串
        /// <summary>
        /// MD5 加密字符串
        /// </summary>
        /// <param name="rawPass">源字符串</param>
        /// <returns>加密后字符串</returns>
        public static string Md5Encoding(string rawPass = "")
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString().ToUpper();
        }
        #endregion
    }
}

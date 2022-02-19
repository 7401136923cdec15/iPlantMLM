using ShrisTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class StringTool
    {
        #region 单实例
        private StringTool() { }
        private static StringTool _Instance;

        public static StringTool Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new StringTool();
                return StringTool._Instance;
            }
        }
        #endregion

        public Object GetValue<T>(T wT, String wProp)
        {
            Object wResult = null;
            try
            {
                PropertyInfo wPropertyInfo = typeof(T).GetProperty(wProp);
                if (wPropertyInfo != null)
                    wResult = wPropertyInfo.GetValue(wT);
                else
                    wResult = "";
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public bool SetValue<T>(T wT, String wProp, Object wValue)
        {
            bool wResult = false;
            try
            {
                PropertyInfo wPropertyInfo = typeof(T).GetProperty(wProp);
                if (wPropertyInfo == null)
                {
                    return wResult;
                }
                wResult = true;
                wPropertyInfo.SetValue(wT, Convert.ChangeType(wValue, wPropertyInfo.PropertyType));

            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public string EncryptString(string str)
        {
            string wResult = string.Empty;
            try
            {
                //创建对象的方法：构造方法，静态方法（工厂）
                MD5 wMd5 = MD5.Create();
                //将字符串转换成字节数组
                byte[] wByteOld = Encoding.UTF8.GetBytes(str);
                //调用加密方法
                byte[] wByteNew = wMd5.ComputeHash(wByteOld);
                //将加密结果进行转换字符串
                StringBuilder wStringBuilder = new StringBuilder();
                foreach (byte wByte in wByteNew)
                    //将字符转换成16进制表示的字符串，而且是恒占用两从头再来
                    wStringBuilder.Append(wByte.ToString("x2"));
                //返回加蜜的字符串
                wResult = wStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        ///  将127.0.0.1形式的IP地址转换成十进制整数
        /// </summary>
        /// <param name="wStrIp"></param>
        /// <returns></returns>
        public long IpToLong(string wStrIp)
        {
            long wResult = 0;
            try
            {
                long[] wIP = new long[4];
                int wPosition1 = wStrIp.IndexOf(".", StringComparison.Ordinal);
                int wPosition2 = wStrIp.IndexOf(".", wPosition1 + 1, StringComparison.Ordinal);
                int wPosition3 = wStrIp.IndexOf(".", wPosition2 + 1, StringComparison.Ordinal);
                // 将每个.之间的字符串转换成整型  
                wIP[0] = long.Parse(wStrIp.Substring(0, wPosition1));
                wIP[1] = long.Parse(wStrIp.Substring(wPosition1 + 1, wPosition2 - wPosition1 - 1));
                wIP[2] = long.Parse(wStrIp.Substring(wPosition2 + 1, wPosition3 - wPosition2 - 1));
                wIP[3] = long.Parse(wStrIp.Substring(wPosition3 + 1));
                //进行左移位处理
                wResult = (wIP[0] << 24) + (wIP[1] << 16) + (wIP[2] << 8) + wIP[3];
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// 将十进制整数形式转换成127.0.0.1形式的ip地址 
        /// </summary>
        /// <param name="wIP"></param>
        /// <returns></returns>
        public string LongToIp(long wIP)
        {
            string wResult = string.Empty;
            try
            {
                StringBuilder wStringBuilder = new StringBuilder();
                //直接右移24位
                wStringBuilder.Append(wIP >> 24);
                wStringBuilder.Append(".");
                //将高8位置0，然后右移16
                wStringBuilder.Append((wIP & 0x00FFFFFF) >> 16);
                wStringBuilder.Append(".");
                //将高16位置0，然后右移8位
                wStringBuilder.Append((wIP & 0x0000FFFF) >> 8);
                wStringBuilder.Append(".");
                //将高24位置0
                wStringBuilder.Append((wIP & 0x000000FF));
                wResult = wStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public bool Ping(string wIP)
        {
            bool wResult = false;
            try
            {
                System.Net.NetworkInformation.Ping wPing = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingOptions wOptions = new System.Net.NetworkInformation.PingOptions();
                wOptions.DontFragment = true;
                string wData = "Test Data!";
                byte[] wBuffer = Encoding.ASCII.GetBytes(wData);
                int wTimeout = 1000; // Timeout 时间，单位：毫秒  
                System.Net.NetworkInformation.PingReply wReply = wPing.Send(wIP, wTimeout, wBuffer, wOptions);
                if (wReply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    wResult = true;
                else
                    wResult = false;
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// DES 加密 注意:密钥必须为８位
        /// </summary>
        /// <param name="wInputString">待加密字符串</param>
        /// <param name="wEncryptKey">密钥</param>
        /// <returns>加密后的字符串</returns>
        public string DesEncrypt(string wInputString, string wEncryptKey)
        {
            string wResult = string.Empty;
            try
            {
                byte[] wByKey = null;
                byte[] wIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                wByKey = Encoding.UTF8.GetBytes(wEncryptKey.Substring(0, 8));
                DESCryptoServiceProvider wDES = new DESCryptoServiceProvider();
                byte[] wInputByteArray = Encoding.UTF8.GetBytes(wInputString);
                using (MemoryStream wMS = new MemoryStream())
                {
                    using (CryptoStream wCS = new CryptoStream(wMS, wDES.CreateEncryptor(wByKey, wIV), CryptoStreamMode.Write))
                    {
                        wCS.Write(wInputByteArray, 0, wInputByteArray.Length);
                        wCS.FlushFinalBlock();
                        wResult = Convert.ToBase64String(wMS.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// DES 解密 注意:密钥必须为８位
        /// </summary>
        /// <param name="wInputString">待解密字符串</param>
        /// <param name="wDecryptKey">密钥</param>
        /// <returns>解密后的字符串</returns>
        public string DesDecrypt(string wInputString, string wDecryptKey)
        {
            string wResult = string.Empty;
            try
            {
                byte[] wByKey = null;
                byte[] wIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] wInputByteArray = new Byte[wInputString.Length];
                wByKey = Encoding.UTF8.GetBytes(wDecryptKey.Substring(0, 8));
                DESCryptoServiceProvider wDes = new DESCryptoServiceProvider();
                wInputByteArray = Convert.FromBase64String(wInputString);
                using (MemoryStream wMS = new MemoryStream())
                {
                    using (CryptoStream wCS = new CryptoStream(wMS, wDes.CreateDecryptor(wByKey, wIV), CryptoStreamMode.Write))
                    {
                        wCS.Write(wInputByteArray, 0, wInputByteArray.Length);
                        wCS.FlushFinalBlock();
                        wResult = Encoding.UTF8.GetString(wMS.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// 获取MD5值
        /// </summary>
        public string GetMD5Hash(string wFileFullName)
        {
            string wResult = string.Empty;
            try
            {
                FileStream wFileStream = new FileStream(wFileFullName, FileMode.Open);
                System.Security.Cryptography.MD5 wMd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] wRetVal = wMd5.ComputeHash(wFileStream);
                wFileStream.Close();

                StringBuilder wStringBuilder = new StringBuilder();
                for (int i = 0; i < wRetVal.Length; i++)
                    wStringBuilder.Append(wRetVal[i].ToString("x2"));

                wResult = wStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// 根据流获取MD5值(注意：此流未关闭)
        /// </summary>
        public string GetMD5Hash(FileStream wFileStream)
        {
            string wResult = string.Empty;
            try
            {
                System.Security.Cryptography.MD5 wMd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] wRetVal = wMd5.ComputeHash(wFileStream);

                StringBuilder wStringBuilder = new StringBuilder();
                for (int i = 0; i < wRetVal.Length; i++)
                    wStringBuilder.Append(wRetVal[i].ToString("x2"));

                wResult = wStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        /// <summary>
        /// 获取MD5值
        /// </summary>
        public string GetMD5Hash(byte[] wByteData)
        {
            string wResult = string.Empty;
            try
            {
                System.Security.Cryptography.MD5 wMd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] wRetVal = wMd5.ComputeHash(wByteData);

                StringBuilder wStringBuilder = new StringBuilder();
                for (int i = 0; i < wRetVal.Length; i++)
                    wStringBuilder.Append(wRetVal[i].ToString("x2"));
                wResult = wStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }

        public string GetMD5HashByStr(string wValue)
        {
            string wResult = string.Empty;
            try
            {
                if (wValue == null || string.IsNullOrWhiteSpace(wValue))
                    return wResult;
                byte[] wByteData = Encoding.Default.GetBytes(wValue);
                MD5 wMd5 = new MD5CryptoServiceProvider();
                byte[] wRetVal = wMd5.ComputeHash(wByteData);

                StringBuilder wStringBuilder = new StringBuilder();
                for (int i = 0; i < wRetVal.Length; i++)
                    wStringBuilder.Append(wRetVal[i].ToString("x2"));
                wResult = wStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
            }
            return wResult;
        }
    }
}

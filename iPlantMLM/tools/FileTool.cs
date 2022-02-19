using ShrisTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iPlantMLM
{
    public class FileTool
    {
        #region 单实例
        private FileTool() { }
        private static FileTool _Instance;
        /// <summary>
        /// 单实例
        /// </summary>
        public static FileTool Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FileTool();
                return FileTool._Instance;
            }
        }
        #endregion

        /// <summary>
        /// 上传文件：要设置共享文件夹是否有创建的权限，否则无法上传文件
        /// </summary>
        /// <param name="fileNamePath">本地文件路径</param>
        /// <param name="urlPath">共享文件夹地址</param>
        public bool UpLoadFile2(string fileNamePath, string urlPath, string User, string Pwd, int islog)
        {
            var flag = false;
            string newFileName = fileNamePath.Substring(fileNamePath.LastIndexOf(@"\") + 1);//取文件名称
            if (urlPath.EndsWith(@"\") == false) urlPath = urlPath + @"\";

            urlPath = urlPath + newFileName;

            WebClient myWebClient = new WebClient();
            NetworkCredential cread = new NetworkCredential(User, Pwd, "Domain");
            myWebClient.Credentials = cread;
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            Stream postStream = null;
            try
            {
                byte[] postArray = r.ReadBytes((int)fs.Length);
                postStream = myWebClient.OpenWrite(urlPath);
                if (postStream.CanWrite)
                {
                    postStream.Write(postArray, 0, postArray.Length);
                    flag = true;
                }
                else
                    flag = false;

                postStream.Close();
                return flag;
            }
            catch (Exception ex)
            {
                if (islog > 0)
                    LoggerTool.SaveException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, System.Reflection.MethodBase.GetCurrentMethod().Name + "()", ex.ToString());
                if (postStream != null)
                    postStream.Close();
                return false;
            }
        }
    }
}

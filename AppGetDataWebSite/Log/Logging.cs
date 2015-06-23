using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGetDataWebSite.Log
{
    public class Logging
    {
        #region Declare Const
        // CSS
        private const string STYLE_SCRIPT =
        @"
        <META http-equiv=Content-Type content =text/html; charset=utf-8> 
        <script src='http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js'>
        </script>
        <style>
            .error
            {
            width:90%;
            margin:3px;
            text-align:justify;
            padding:5px;
            border:1px solid red;
            border-radius:5px;
            -webkit-border-radius:5px;
            display:none;
            }
            .watch
            {
            width:90%;
            margin:3px;
            text-align:justify;
            padding:5px;
            border:1px solid blue;
            border-radius:5px;
            -webkit-border-radius:5px;
            display:none;
            }
            .trace
            {
            width:90%;
            margin:3px;
            text-align:justify;
            padding:5px;
            border:1px solid green;
            border-radius:5px;
            -webkit-border-radius:5px;
            display:none;
            }
            .charge
            {
            width:90%;
            margin:3px;
            text-align:justify;
            padding:5px;
            border:1px solid black;
            border-radius:5px;
            -webkit-border-radius:5px;
            display:none;
            }
        </style>";
        public const string ERROR = "error";
        public const string WATCH = "watch";
        public const string TRACE = "trace";
        public const string CHARGE = "charge";

        //
        #endregion
        #region Write log

        public static bool Write(string errorType, params string[] arrMess)
        {
            bool bRet = false;
            FileStream fileStream = null;
            DirectoryInfo directoryInfo;
            DateTime dFile = DateTime.Now;
            try
            {
                var appDomain = System.AppDomain.CurrentDomain;
                var basePath = appDomain.BaseDirectory;
                var sPath = Path.Combine(basePath, "Log\\" + dFile.ToString("yyyy") + "\\" + dFile.ToString("MM"));
                directoryInfo = new DirectoryInfo(sPath);
                if (directoryInfo.Exists == false)
                {
                    //Neu khong ton tai thu muc ghi log 
                    directoryInfo.Create();//Tao moi thu muc ghi log

                }
                var fileLog = sPath + "\\" + autoRenameFile(sPath, "ApzERP-Log-" + dFile.ToString("yyyyMMdd")) + ".txt";

                string strInfor = errorType + "\n" + dFile.ToString("yyyy-MM-dd-HH:mm:ss:fff") + "\n";

                fileStream = new FileStream(fileLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                foreach (string mess in arrMess)
                {
                    strInfor += mess + "\n"; //cac tham so truyen vao
                }
                //strInfor += "</div>";
                fileStream.Write(Encoding.UTF8.GetBytes(strInfor), 0, Encoding.UTF8.GetByteCount(strInfor));//Ghi dong log vao file
                fileStream.Close();
                fileStream.Dispose();
            }
            catch
            {
                bRet = false;
            }
            finally
            {
                GC.Collect();
            }
            bRet = true;

            return bRet;
        }

        private static string autoRenameFile(string folderPath, string fileName)
        {
            try
            {
                string[] allFiles = Directory.GetFiles(folderPath).Select(filename => Path.GetFileNameWithoutExtension(filename)).ToArray();

                if (allFiles.Length == 0)
                {
                    return fileName;
                }
                FileInfo fileInfo = null;
                if (allFiles.Length == 1)
                    fileInfo = new FileInfo(folderPath + "\\" + fileName + ".html");
                else
                    fileInfo = new FileInfo(folderPath + "\\" + String.Format("{0} ({1})", fileName, allFiles.Length - 1) + ".html");
                if (fileInfo.Length >= 1048576)
                {
                    fileName = String.Format("{0} ({1})", fileName, allFiles.Length);
                }
                else
                    if (allFiles.Length != 1)
                        fileName = String.Format("{0} ({1})", fileName, allFiles.Length - 1);
                return fileName;
            }
            catch (Exception)
            {
                return fileName;
            }
        }
        #endregion
    }
}

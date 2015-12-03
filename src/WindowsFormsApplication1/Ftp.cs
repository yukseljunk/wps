using System;
using System.IO;
using System.Net;
using PttLib.Helpers;
using WordPressSharp.Models;

namespace WindowsFormsApplication1
{
    public class FtpConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
    }
    
    public class Ftp
    {
        public void UploadFileFtp(Data file, string ftpAddress, string username, string password)
        {
            var tryAgain = true;
            while (tryAgain)
            {
                var request = (FtpWebRequest) WebRequest.Create("ftp://"+ftpAddress + "/" + Path.GetFileName(file.Name));
                try
                {
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(username, password);
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.KeepAlive = false;
                    request.ServicePoint.Expect100Continue = false;
                    //request.Timeout = 1000000;

                    using (Stream reqStream = request.GetRequestStream())
                    {
                        //reqStream.ReadTimeout = 3000000;
                        //reqStream.WriteTimeout = 3000000;
                        reqStream.Write(file.Bits, 0, file.Bits.Length);
                        reqStream.Flush();
                        reqStream.Close();
                    }
                    tryAgain = false;
                }
                catch (Exception exception)
                {
                    Logger.LogExceptions(exception);
                    if(exception.Message.Contains("Not logged in"))
                    {
                        break;
                    }
                }
                finally
                {
                    request.Abort();
                    request = null;
                    GC.Collect();
                }

            }

        }

        public void UploadFileFtp(string filePath, string ftpAddress, string username, string password)
        {
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpAddress + "/" + Path.GetFileName(filePath));
            try
            {
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                byte[] buffer;
                using (FileStream stream = File.OpenRead(filePath))
                {
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Flush();
                    stream.Close();
                }
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(buffer, 0, buffer.Length);
                    reqStream.Flush();
                    reqStream.Close();
                }
            }
            finally
            {
                request.Abort();
                request = null;
                GC.Collect();
            }
        }

        public void MakeFtpDir(string ftpAddress, string pathToCreate, string username, string password)
        {
            FtpWebRequest reqFTP = null;
            
            string[] subDirs = pathToCreate.Split('/');

            string currentDir = string.Format("ftp://{0}", ftpAddress);

            foreach (string subDir in subDirs)
            {
                try
                {
                    currentDir = currentDir + "/" + subDir;
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(currentDir);
                    try
                    {
                        reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                        reqFTP.UseBinary = true;
                        reqFTP.KeepAlive = false;
                        reqFTP.Credentials = new NetworkCredential(username, password);
                        using (FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse())
                        {
                            using (Stream ftpStream = response.GetResponseStream())
                            {
                                ftpStream.Flush();
                                ftpStream.Close();
                            }
                            response.Close();
                        }
                    }
                    finally
                    {
                        reqFTP.Abort();
                        reqFTP = null;
                        GC.Collect();
                    }
                }
                catch (Exception ex)
                {
                    //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                }
            }
        }

        public string TestConnection(FtpConfig ftpConfiguration)
        {
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpConfiguration.Url + "/");
            request.Credentials = new NetworkCredential(ftpConfiguration.UserName, ftpConfiguration.Password);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    response.Close();
                    return "";
                }
            }
            catch(Exception exception)
            {
                return exception.ToString();
            }
            finally
            {
                request.Abort();
                request = null;
                GC.Collect();
            }
        }
    }
}
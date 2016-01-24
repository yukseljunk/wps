using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using PttLib.Helpers;
using WordPressSharp.Models;

namespace WordpressScraper.Ftp
{
    public class Ftp : IFtp
    {
        private readonly FtpConfig _ftpConfiguration;

        public Ftp(FtpConfig ftpConfiguration)
        {
            _ftpConfiguration = ftpConfiguration;
        }

        public string DeleteDirectory(string path)
        {
            var clsRequest = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + path);
            clsRequest.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);

            var filesList = DirectoryListing(path);

            foreach (string file in filesList)
            {
                if(file=="." || file==".."){continue;}
                DeleteFile(path + file);
            }

            clsRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

            string result = string.Empty;
            using (FtpWebResponse response = (FtpWebResponse)clsRequest.GetResponse())
            {
                long size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                        datastream.Close();
                        response.Close();
                    }
                }
            }
            return result;
        }

        public List<string> DirectoryListing(string path)
        {
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + path);
            request.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);

            request.Method = WebRequestMethods.Ftp.ListDirectory;
            List<string> result = new List<string>();

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            result.Add(reader.ReadLine());
                        }

                        reader.Close();
                        response.Close();
                    }
                }
            }

            return result;
        }

        public string DeleteFile(string path)
        {
            FtpWebRequest clsRequest = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + path);
            clsRequest.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);

            clsRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            string result = string.Empty;
            using (FtpWebResponse response = (FtpWebResponse)clsRequest.GetResponse())
            {
                long size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                        datastream.Close();
                        response.Close();
                    }
                }
            }
            return result;
        }

        public void UploadFileFtp(Data file, string folder)
        {
            var tryAgain = true;
            while (tryAgain)
            {
                var request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + folder + "/" + Path.GetFileName(file.Name));
                try
                {
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);
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
                    if (exception.Message.Contains("Not logged in"))
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

        public void UploadFileFtp(string filePath, string folder)
        {
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + folder + "/" + Path.GetFileName(filePath));
            try
            {
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);
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

        public void MakeFtpDir(string pathToCreate)
        {
            FtpWebRequest reqFTP = null;

            string[] subDirs = pathToCreate.Split('/');

            string currentDir = string.Format("ftp://{0}", _ftpConfiguration.Url);

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
                        reqFTP.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);
                        using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
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

        public string TestConnection()
        {
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/");
            request.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    response.Close();
                    return "";
                }
            }
            catch (Exception exception)
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
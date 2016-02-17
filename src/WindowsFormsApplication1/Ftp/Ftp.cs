using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
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

        private BackgroundWorker _bw;
        private string _path;
        public string DeleteDirectory(string path)
        {
            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _path = path;
            _bw.DoWork += (obj, e) => GetDirectoryListing(path, e);
            _bw.ProgressChanged += DirectoryListingProgress;
            _bw.RunWorkerCompleted += GotDirectoryListing;
            _bw.RunWorkerAsync();

            return "";
        }

        public event EventHandler<string> DirectoryListingProgressing;

        public void OnDirectoryListingProgressing(string e)
        {
            EventHandler<string> handler = DirectoryListingProgressing;
            if (handler != null) handler(this, e);
        }

        public event EventHandler DirectoryListingFetchFinished;

        public void OnDirectoryListingFetchFinished(EventArgs e)
        {
            EventHandler handler = DirectoryListingFetchFinished;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<string> DirectoryDeletionProgressing;

        public void OnDirectoryDeletionProgressing(string e)
        {
            EventHandler<string> handler = DirectoryDeletionProgressing;
            if (handler != null) handler(this, e);
        }

        public event EventHandler DirectoryDeletionFinished;

        public void OnDirectoryDeletionFinished(EventArgs e)
        {
            EventHandler handler = DirectoryDeletionFinished;
            if (handler != null) handler(this, e);
        }

        public void CancelAsync()
        {
            if (_bw.IsBusy)
            {
                _bw.CancelAsync();
            }
        }


        private void GotDirectoryListing(object sender, RunWorkerCompletedEventArgs e)
        {
            OnDirectoryListingFetchFinished(null);

            if(e.Cancelled)
            {
                return;
            }

            var directoryRes = (FtpDirectory) e.Result;
            if (directoryRes.Files == null && directoryRes.Folders==null)
            {
                OnDirectoryDeletionFinished(null);
                return;
            }
            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _bw.DoWork += (obj, e2) => DeleteDirectory(directoryRes, e2);
            _bw.ProgressChanged += DirectoryDeletionProgress;
            _bw.RunWorkerCompleted += FinishedDirectoryDeletion;
            _bw.RunWorkerAsync();


        }

        private void FinishedDirectoryDeletion(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled)
            {
                OnDirectoryDeletionFinished(null);
                return;

            }
            DeleteDirectory(_path);
        }

        private void DirectoryDeletionProgress(object sender, ProgressChangedEventArgs e)
        {
            OnDirectoryDeletionProgressing(e.UserState.ToString());
        }

        private void DeleteDirectory(FtpDirectory directory, DoWorkEventArgs e)
        {
            if (directory.Folders == null && directory.Files == null) return;

            //delete files on this directory first
            if (directory.Files != null)
            {
                foreach (var ftpFile in directory.Files)
                {
                    if(_bw.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    DeleteFile(ftpFile.FullPath);
                    _bw.ReportProgress(0, ftpFile.FullPath + " deleted");
                }
            }
            if (directory.Folders == null) return;
            foreach (var folder in directory.Folders)
            {
                if (_bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                DeleteDirectory(folder, e);

                if (_bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                
                DeleteEmptyDirectory(folder.FullPath);
                _bw.ReportProgress(0, folder.FullPath + " deleted");

            }
        }

        private void DirectoryListingProgress(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState == null) return;
            OnDirectoryListingProgressing(e.UserState.ToString());
        }

        private FtpDirectory GetDirectoryListing(string path, DoWorkEventArgs e)
        {
            var res = new FtpDirectory();
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + path);
            request.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
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
            foreach (var item in result)
            {
                if (_bw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                var regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                var match = regex.Match(item);
                if (match.Success)
                {
                    var type = match.Groups[1].Value;
                    var name = match.Groups[6].Value;
                    if (name == "." || name == "..") continue;
                    if (type == "d")
                    {
                        if (res.Folders == null) res.Folders = new List<FtpDirectory>();
                        _bw.ReportProgress(0, "Reading " + path + "/" + name);
                        var subFolder = GetDirectoryListing(path + "/" + name, e);
                        if (subFolder.Folders != null)
                        {
                            _bw.ReportProgress(0, "Found " + subFolder.Folders.Count + " folders");
                        }
                        if (subFolder.Files != null)
                        {
                            _bw.ReportProgress(0, "Found " + subFolder.Files.Count + " files");
                        }
                        subFolder.Name = name;
                        subFolder.FullPath = path + "/" + name;
                        res.Folders.Add(subFolder);
                    }
                    else
                    {
                        if (res.Files == null) res.Files = new List<FtpFile>();
                        res.Files.Add(new FtpFile() { Name = name, FullPath = path + "/" + name });
                        //_bw.ReportProgress(0, "Found " + path + "/" + name);
                    }
                }
            }
            e.Result = res;
            return res;
        }


        public void DeleteDirectory(FtpDirectory directory)
        {
            if (directory.Folders == null && directory.Files == null) return;

            //delete files on this directory first
            if (directory.Files != null)
            {
                foreach (var ftpFile in directory.Files)
                {
                    DeleteFile(ftpFile.FullPath);
                }
            }
            if (directory.Folders == null) return;
            foreach (var folder in directory.Folders)
            {
                DeleteDirectory(folder);
                DeleteEmptyDirectory(folder.FullPath);
            }

        }

        public string DeleteEmptyDirectory(string path)
        {
            Debug.WriteLine("Delete empty dir ftp://" + _ftpConfiguration.Url + "/" + path);

            var clsRequest = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + path);
            clsRequest.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);

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

        public FtpDirectory FullDirectoryListing(string path)
        {
            var res = new FtpDirectory();
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + path);
            request.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
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
            foreach (var item in result)
            {
                var regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                var match = regex.Match(item);
                if (match.Success)
                {
                    var type = match.Groups[1].Value;
                    var name = match.Groups[6].Value;
                    if (name == "." || name == "..") continue;
                    if (type == "d")
                    {
                        if (res.Folders == null) res.Folders = new List<FtpDirectory>();
                        var subFolder = FullDirectoryListing(path + "/" + name);
                        subFolder.Name = name;
                        subFolder.FullPath = path + "/" + name;
                        res.Folders.Add(subFolder);
                    }
                    else
                    {
                        if (res.Files == null) res.Files = new List<FtpFile>();
                        res.Files.Add(new FtpFile() { Name = name, FullPath = path + "/" + name });
                    }
                }
            }
            return res;
        }

        public string RenameFile(string path, string newName)
        {
            var result = "";
            var request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + path);
            request.Credentials = new NetworkCredential(_ftpConfiguration.UserName, _ftpConfiguration.Password);

            request.Method = WebRequestMethods.Ftp.Rename;
            request.RenameTo = newName;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
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
            Debug.WriteLine("Delete ftp://" + _ftpConfiguration.Url + "/" + path);
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

        private Dictionary<string, List<string>> _fileList= new Dictionary<string, List<string>>();

        public void UploadFileFtp(string filePath, string folder)
        {
            var fileExists = false;
            var filenameToUpload = filePath;
            var fileInfo = new FileInfo(filePath);

            if (!_fileList.ContainsKey(folder))
            {
                _fileList.Add(folder, DirectoryListing(folder));
            }
            if (_fileList[folder].Contains(fileInfo.Name))
            {
                fileExists = true;
                //file exists, so upload with a new name    
                filenameToUpload += ".tmp";
                if (_fileList[folder].Contains(fileInfo.Name+".tmp"))
                {
                    try
                    {
                        DeleteFile(folder + "/" + fileInfo.Name + ".tmp");
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }


            var request = (FtpWebRequest)WebRequest.Create("ftp://" + _ftpConfiguration.Url + "/" + folder + "/" + Path.GetFileName(filenameToUpload));
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
            catch (Exception)
            {
                return;
            }
            finally
            {
                request.Abort();
                request = null;
                GC.Collect();
            }

            if (fileExists)
            {
                try
                {
                    DeleteFile(folder + "/" + fileInfo.Name);
                }
                catch
                {
                    // ignored
                }
                try
                {
                    RenameFile(folder + "/" + fileInfo.Name + ".tmp", fileInfo.Name);
                }
                catch
                {
                    // ignored
                }
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
using WordPressSharp.Models;

namespace WordpressScraper.Ftp
{
    public interface IFtp
    {
        void UploadFileFtp(Data file, string folder);
        void UploadFileFtp(string filePath, string folder);
        void MakeFtpDir(string ftpAddress);
        string TestConnection();
    }
}
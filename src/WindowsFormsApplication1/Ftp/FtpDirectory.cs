using System.Collections.Generic;

namespace WordpressScraper.Ftp
{
    public class FtpDirectory
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public List<FtpFile> Files { get; set; }
        public List<FtpDirectory> Folders { get; set; }
    }
}
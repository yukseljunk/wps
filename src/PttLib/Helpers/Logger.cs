using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using PttLib.Operators.Query;

namespace PttLib.Helpers
{
    public static class Logger
    {
        private static string _logFilePath = Path.GetFullPath(Path.Combine(Helper.AssemblyDirectory, @"logs\log.txt"));
        public static string LogFilePath
        {
            get { return Path.GetFullPath(_logFilePath); }
        }

        public static string LogDirectory
        {
            get
            {
                return Path.GetDirectoryName(_logFilePath);
            }
        }

        public static List<string> LogFiles
        {
            get
            {
                return Directory.GetFiles(LogDirectory)
                    .Select(x => new FileInfo(x))
                    .OrderByDescending(x => x.LastWriteTime)
                    .Select(fi=>fi.Name)
                    .ToList();
            }
        }


        public static void InitialLogging(Query query, IList<string> operators, IList<string> hotels)
        {
            CreateLogFolder();
            ResetLogFile();
            LogProcess("App Version:"+Helper.GetVersion());
            query.SetCultureInfo(new CultureInfo("tr-TR"));
            var xmlSerializer = new XmlSerializer();
            var serializedQuery = xmlSerializer.Serialize(query);
            LogProcess("Query:");
            LogProcess(serializedQuery);
            LogProcess("-----");
        }

        public static void CreateLogFolder()
        {
            if (Directory.Exists(LogDirectory)) return;
            Directory.CreateDirectory(LogDirectory);

        }

        public static void LogExceptions(Exception exception)
        {
            if (exception == null) return;
            var data = OneLine("EXCEPTION ENCOUNTERED")+Environment.NewLine;

            var webException = exception as WebException;
            if (exception.InnerException !=null && webException == null)
            {
                webException = exception.InnerException as WebException;
            }
            if(webException!=null)
            {
                if (webException.Status == WebExceptionStatus.ProtocolError)
                {
                    using (var resp = webException.Response)
                    {
                        try
                        {
                            using (var sr = new StreamReader(resp.GetResponseStream()))
                            {
                                data += OneLine("Web Exception Details:" + Environment.NewLine + sr.ReadToEnd()) + Environment.NewLine;

                                sr.Close();
                            }

                        }
                        catch (Exception exc)
                        {
                            data += OneLine("Another exception: " + exc.Message) + Environment.NewLine;
                        }
                        resp.Close();
                    }
                }
            }

            data += OneLine(exception.Message + Environment.NewLine + exception.StackTrace);
            if(exception.InnerException!=null)
            {
                data += Environment.NewLine + OneLine(exception.InnerException.Message + Environment.NewLine + exception.InnerException.StackTrace) + Environment.NewLine;
            }
            var fileReaderWriter = new FileReaderWriter();
            fileReaderWriter.WriteFile(LogFilePath, data);
        }

        public static void LogProcess(string process)
        {
            if (String.IsNullOrEmpty(process)) return;
            var fileReaderWriter = new FileReaderWriter();
            fileReaderWriter.WriteFile(LogFilePath, OneLine(process));
        }

        private static string OneLine(string description)
        {
            return DateTimeColumn + "\t\t" + System.Threading.Thread.CurrentThread.ManagedThreadId + "\t" + description;
        }

        private static string DateTimeColumn
        {
            get
            {
                return DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "." + DateTime.Now.Hour +
                       "." +
                       DateTime.Now.Minute + "." + DateTime.Now.Second;
            }
        }
        public static void OpenLogFileInShell()
        {
            Helper.OpenFileInShell(LogFilePath);
        }

        public static void ResetLogFile()
        {
            var data = Environment.NewLine + "DateTime\t\tThreadId\tDescription";

            data += Environment.NewLine + "--------\t\t--------\t-----------";
            data += Environment.NewLine + OneLine("Start") + Environment.NewLine;
            
            var fileReaderWriter = new FileReaderWriter();

            _logFilePath = Path.GetFullPath(Path.Combine(Helper.AssemblyDirectory,
                                @"..\..\logs\log_" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "." + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".txt"));
            fileReaderWriter.WriteFile(LogFilePath, data);
        }
    }
}
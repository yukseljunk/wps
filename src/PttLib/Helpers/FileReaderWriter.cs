using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace PttLib.Helpers
{
    public class FileReaderWriter
    {
        /// <summary>
        /// read file and return the content
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>null if not found</returns>
        public string ReadFile(string filePath)
        {
            return ReadFile(filePath, false);
        }

        /// <summary>
        /// the same as ReadFile
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="trimSpaces">If the trailing and ending spaces will be removed</param>
        /// <returns></returns>
        public string ReadFile(string filePath, bool trimSpaces)
        {
            var readFile = ReadFile(filePath, trimSpaces, "");
            return readFile == null ? null : readFile.ToString();
        }

        public StringBuilder ReadFile(string filePath, string dummy)
        {
            return ReadFile(filePath, false, "");
        }

        public StringBuilder ReadFile(string filePath, bool trimSpaces, string dummy)
        {
            var stringBuilder = new StringBuilder();
            var readLines = ReadFile(filePath, trimSpaces, null, null);
            foreach (var readLine in readLines)
            {
                stringBuilder.Append(readLine);
            }
            return stringBuilder;
        }

        
        public List<string> ReadFile(string filePath, bool trimSpaces, string dummy, string dummy2)
        {
            
            var filePathWOutProtocol = Regex.Replace(filePath, "file://", "", RegexOptions.IgnoreCase);
            filePathWOutProtocol = Regex.Replace(filePathWOutProtocol, "file:\\\\", "", RegexOptions.IgnoreCase);

            var fileExists = File.Exists(filePathWOutProtocol);
            //guard
            if (!fileExists)
            {
                throw new FileNotFoundException(filePathWOutProtocol);
            }

            var fileContent = new List<string>();
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (var sr = new StreamReader(filePathWOutProtocol))
                {
                    String line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        var refinedLine = trimSpaces ? line.Trim() : line;
                        fileContent.Add(refinedLine);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return fileContent;
        }

        
        /// <summary>
        /// write content to a file 
        /// </summary>
        /// <param name="filePath">if file found, appends the new content</param>
        /// <param name="content"></param>
        public void WriteFile(string filePath, string content)
        {
            WriteFile(filePath, new List<string>(){content});
        }

        /// <summary>
        /// same as WriteFile, content is sent as stringbuilder
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public void WriteFile(string filePath, StringBuilder content)
        {
           WriteFile(filePath, content.ToString());

        }


        private static Mutex mut = new Mutex();


        /// <summary>
        /// write strings of array by adding newline inbetween
        /// </summary>
        /// <param name="filePath">if file found, appends the new content</param>
        /// <param name="content"></param>
        public void WriteFile(string filePath, List<string> content)
        {
            var fileExists = File.Exists(filePath);
            
            // Wait until it is safe to enter.
            mut.WaitOne();
            if (!fileExists)
            {
              


                try
                {
                    // Create a file to write to.
                    //using (var sw = TextWriter.Synchronized(File.CreateText(filePath)))
                    using (var sw = (File.CreateText(filePath)))
                    {
                        foreach (var line in content)
                        {
                            sw.WriteLine(line);

                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }

               
            }
            else
            {
                AppendToFile(filePath, content);
            }
            // Release the Mutex.
            mut.ReleaseMutex();


        }


        /// <summary>
        /// clears file contents
        /// </summary>
        /// <param name="filePath">If not found, throws exception</param>
        public void ClearFileContent(string filePath)
        {
            var fileExists = File.Exists(filePath);
            if (!fileExists) throw new FileNotFoundException("File not found:" + filePath);

            using (var sw = new StreamWriter(filePath))
            {
                sw.Write("");
            }

        }

        public void AppendToFile(string filePath, string content)
        {
            AppendToFile(filePath, new List<string>(){content});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">if not found, throws exception</param>
        /// <param name="content"></param>
        public void AppendToFile(string filePath, StringBuilder content)
        {
           AppendToFile(filePath, content.ToString());
        }

        /// <summary>
        /// appends strings of array by adding newline inbetween
        /// </summary>
        /// <param name="filePath">if not found, throws exception</param>
        /// <param name="content"></param>
        public void AppendToFile(string filePath, List<string> content)
        {
            var fileExists = File.Exists(filePath);
            if (!fileExists) throw new FileNotFoundException("File not found:" + filePath);

            using (var sw = File.AppendText(filePath))
            {
                foreach (var line in content)
                {
                    sw.WriteLine(line);
                }
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Ionic.Zip;
using Ionic.Zlib;

namespace PttLib.Helpers
{
    public class Helper
    {
        public static string PROCESS_MESSAGE_INFO_START_TOKEN = "msg:";
        private static readonly string _versionFilePath = AssemblyDirectory + @"\current_version.txt";

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }

        public static void OpenFileInShell(string fileName)
        {
            var psi = new ProcessStartInfo(fileName);
            psi.UseShellExecute = true;
            Process.Start(psi);
        }
        
        static public bool RemoteServerRunNeeded(string region)
        {
            return region == "Turkey" || region == "Greece" || region == "Spain";
        }

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string GetVersion()
        {
            if (File.Exists(_versionFilePath))
            {
                return GetFileContent(_versionFilePath);
            }

            return "";
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GetFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found: " + filePath);
            }

            var _fileReaderWriter = new FileReaderWriter();
            string fileContent = "";

            try
            {
                fileContent = _fileReaderWriter.ReadFile(filePath, true);

            }
            catch (Exception e)
            {

                throw e;
            }
            return fileContent;
        }


        public static DateTime FormatDateTime(string dateTimeString)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            string format = "dd.MM.yyyy";
            return DateTime.ParseExact(dateTimeString, format, provider);
        }

        public static string ToUpperFirstLetter(string source)
        {
            return source.ToLower();
        }

        public static string Encode(string str)
        {
            var charClass = String.Format("0-9a-zA-Z{0}", Regex.Escape("-_.!~*'()"));
            return Regex.Replace(str,
                String.Format("[^{0}]", charClass),
                new MatchEvaluator(EncodeEvaluator));
        }

        public static string EncodeEvaluator(Match match)
        {
            return (match.Value == " ") ? "+" : String.Format("%{0:X2}", Convert.ToInt32(match.Value[0]));
        }


        /// <summary>
        /// refine hotel name, e.g. remove unnecessary stars, paranthesis, star number etc.
        /// </summary>
        /// <param name="hotelName"></param>
        /// <returns></returns>
        public static string RefineHotelName(string hotelName)
        {
            var result = hotelName.Replace((char) 160, (char) 32);

            //5* yerine 5***** yazanlari tek yildiza indir
            result = result.Replace(" *****", "*").Replace(" ****", "*").Replace(" ***", "*").Replace(" **", "*").Replace(" *", "*").Replace("#", "");
            for (var starIndex = 2; starIndex < 6; starIndex++)
            {
                result = result.Replace(starIndex + "+*", "");
                result = result.Replace(starIndex + "+ *", "");
                result = result.Replace(starIndex + " +*", "");
                result = result.Replace(starIndex + " + *", "");
                result = result.Replace(" " + starIndex + "*+", "");
                result = result.Replace(" " + starIndex + "*", "");
            }
            result = result.Replace("      ", " ").Replace("     ", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");


            //gelen: ADORA GOLF RESORT HOTEL (5*), BELEK
            var match = Regex.Match(result, @"([^\(,]*)[\(,]", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                result = match.Groups[1].Value.Trim();
            }
            /*nnatalie апт.*/
            result = result.Replace("апт.", "");


            /*
            //natalie gelen :апт., alttaki kod yalniz turkce karakterleri de ucurabiliyor, onun icin simdilik disabled.
            match = Regex.Match(result, @"([\x00-\x7F]*)[^\x00-\x7F]+", RegexOptions.IgnoreCase);
            if (match.Success) 
            {
                result = match.Groups[1].Value.Trim();
            }
            */

            return result.Trim();
        }




        /// <summary>
        /// finds best matches in the list
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lookupList"></param>
        /// <returns></returns>
        public static List<string> GetBestMatches(string input, List<string> lookupList, List<string> exclusionList)
        {
            var result = new List<string>();
            var wordsInInput = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var lookupOccurence = new List<int>();
            for (var lookupIndex = 0; lookupIndex < lookupList.Count; lookupIndex++)
            {
                var wordsInLookupItem = new HashSet<string>(lookupList[lookupIndex].ToUpper(new CultureInfo("en-US")).Split(new string[] { " " },
                                                                      StringSplitOptions.RemoveEmptyEntries));
                wordsInLookupItem.IntersectWith(wordsInInput);
                wordsInLookupItem.ExceptWith(exclusionList);
                wordsInLookupItem.RemoveWhere(i => i.Length == 1);
                lookupOccurence.Add(wordsInLookupItem.Count);
            }

            var sorted = lookupOccurence
               .Select((x, i) => new KeyValuePair<int, int>(x, i))
               .OrderByDescending(x => x.Key)
               .ToList();

            var valueOrdered = sorted.Select(x => x.Key).ToList();
            var keyOrdered = sorted.Select(x => x.Value).ToList();

            for (var i = 0; i < valueOrdered.Count; i++)
            {
                if (valueOrdered[i] == 0) break;
                result.Add(lookupList[keyOrdered[i]]);
            }
            return result;
        }


        private static Cursor _currentCursor;

        public static void SetBusyMouseCursor()
        {
            _currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }
        public static void ResetMouseCursor()
        {
            Cursor.Current = _currentCursor;
        }

        public static void Zip(IList<string> files, string zipFileName)
        {
            if(files==null) throw new ArgumentException("cannot create zip from null","files");
            if (zipFileName == "") throw new ArgumentException("zipfilename must be specified", "zipFileName");

            using (var zip = new ZipFile())
            {
                zip.CompressionLevel=CompressionLevel.BestCompression;
                zip.AddFiles(files, false,"");   
                zip.Save(zipFileName);
            }

        }
    }
}

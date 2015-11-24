using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PttLib.CaptchaBreaker.Info;
using System.Drawing;
using PttLib.Helpers;

namespace PttLib.CaptchaBreaker.Tez
{
    public class TezBreaker: ICaptchaBreaker
    {
        private readonly CaptchaBreakerFactory _captchaBreakerFactory;
        private const string templatesFolder = "/CaptchaBreaker/Tez/CompareSet";
        private const string patternCharacterOrder = "2834657bdfhkgypwemnrxc";

        private static readonly Dictionary<char, List<Tuple<int, bool[]>>> _captchaCache = new
            Dictionary<char, List<Tuple<int, bool[]>>>();

        private static int MAX_MATCH_COUNT = 5;

        public TezBreaker(CaptchaBreakerFactory captchaBreakerFactory)
        {
            _captchaBreakerFactory = captchaBreakerFactory;
        }

        public void Prepare()
        {
            string[] filePaths = Directory.GetFiles(Helper.AssemblyDirectory + templatesFolder);
            foreach (var filePath in filePaths)
            {
                var fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                var bitmap = new Bitmap(filePath);
                var fileBitInfo = BitArray.ImageBitArray(bitmap);
                var firstChar = fileNameOnly[0];
                if (_captchaCache.ContainsKey(firstChar))
                {
                    _captchaCache[firstChar].Add(new Tuple<int, bool[]>(bitmap.Height, fileBitInfo));

                }
                else
                {
                    _captchaCache[firstChar] = new List<Tuple<int, bool[]>>() { new Tuple<int, bool[]>(bitmap.Height, fileBitInfo) };
                }

            }
        }


        public string Guess(string fileName, int tryCount)
        {
           // Logger.LogProcess(string.Format("{1} captcha try {0} for {2}", tryCount, this.Name, fileName));

            if (tryCount > MaxNumberOfTry)
            {
                return _captchaBreakerFactory.ManualBreaker().Guess(fileName, tryCount - MaxNumberOfTry);
            }
            var guess = Guess(new Bitmap(fileName));
            //Logger.LogProcess(string.Format("{2} captcha guess {1} for try {0} for {3}", tryCount, guess ?? "", this.Name, fileName));
            return guess;
        }

        public string Guess(Bitmap bitmap)
        {
            if (bitmap == null) return null; 
            var captchaImage = BitArray.ImageBitArray(bitmap);
            List<Tuple<char, Point>> matches = new List<Tuple<char, Point>>();
            foreach (var patternCharacter in patternCharacterOrder)
            {
                foreach (var patternImageBitHeightCouple in _captchaCache[patternCharacter])
                {
                    var points =  Info.BitArray.FindBitBitmap(captchaImage, patternImageBitHeightCouple.Item2, bitmap.Height, patternImageBitHeightCouple.Item1);
                    var patternImageWidth = patternImageBitHeightCouple.Item2.Length / patternImageBitHeightCouple.Item1;
                    foreach (var point in points)
                    {
                        matches.Add(new Tuple<char, Point>(patternCharacter, point));
                        Info.BitArray.FalsifyBitVertical(captchaImage, bitmap.Width, point.X + 2, patternImageWidth - 4);
                        //MessageBox.Show(TezCaptchaBreaker.ImageBitArrayRepr(captchaImage, image.Width));

                    }
                    if (matches.Count == MAX_MATCH_COUNT)
                    {
                        break;
                    }
                }
                if (matches.Count == MAX_MATCH_COUNT)
                {
                    break;
                }
            }
            var result = "";
            if (matches.Count > 0)
            {
                var matchesHorizontally = matches.OrderBy(m => m.Item2.X).ToList();
                foreach (var match in matchesHorizontally)
                {
                    result += match.Item1;
                }

            }
            return result;

        }


    

        public string Name { get { return "Tez"; } }
        public bool IsManual
        {
            get { return false; }
        }

        public int MaxNumberOfTry { get { return 2; } }
    }
}

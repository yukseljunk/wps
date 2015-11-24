using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using PttLib.CaptchaBreaker.Fx;
using PttLib.CaptchaBreaker.Info;
using PttLib.Helpers;

namespace PttLib.CaptchaBreaker.Coral.Guesser
{
    abstract class CaptchaGuess
    {
        protected string TrainingDataMainFolder = Helper.AssemblyDirectory + @"\CaptchaBreaker\Coral\CompareSet\";
        protected static List<Dictionary<int, List<IntVector>>> FillRatios = new List<Dictionary<int, List<IntVector>>>();

        protected static List<FillPercentageTune> Tunes;
        protected static bool _isReady;
        public static bool IsReady
        {
            get { return _isReady; }
        }
        public void Prepare()
        {
            _isReady = false;
            Tunes = new List<FillPercentageTune>();
            Tunes.Add(new FillPercentageTune() { BestFit = false, Division = 20, ResizeDimension = 60 });
            //_tunes.Add(new FillPercentageTune() { BestFit = false, Division = 5, ResizeDimension = 0 });
            // _tunes.Add(new FillPercentageTune() { BestFit = true, Division = 20, ResizeDimension = 60 });
            //_tunes.Add(new FillPercentageTune() { BestFit = true, Division = 10, ResizeDimension = 40 });
            //          _tunes.Add(new FillPercentageTune() { BestFit = false, Division = 10, ResizeDimension = 40 });

            ReadFillRatiosOfFiles(Tunes);
        }

        protected  void ReadFillRatiosOfFiles(List<FillPercentageTune> tunes)
        {
            Logger.LogProcess("Started reading captcha templates");

            FillRatios = new List<Dictionary<int, List<IntVector>>>();

            foreach (var tune in tunes)
            {

                var fillRatiosForTune = new Dictionary<int, List<IntVector>>();

                for (int numberIndexor = 0; numberIndexor < 10; numberIndexor++)
                {
                    Logger.LogProcess("Read captcha templates for "+numberIndexor);
                    var ratios = new List<IntVector>();
                    var filePaths = Directory.GetFiles(TrainingDataMainFolder + numberIndexor + @"\", "*.png");
                    foreach (var filePath in filePaths)
                    {
                        var bitmap = new Bitmap(filePath);

                        var bitmapFillInfo = ReadBitmapFillPercentages(bitmap, Histogram.BgColor(bitmap), tune);
                        ratios.Add(bitmapFillInfo);

                    }

                    fillRatiosForTune.Add(numberIndexor, ratios);

                }
                FillRatios.Add(fillRatiosForTune);
            }
            Logger.LogProcess("Finished reading captcha templates");

        }


        protected IntVector ReadBitmapFillPercentages(Bitmap bitmap, string bgColor, FillPercentageTune tune)
        {
            var filled = new List<int>() { };
            var allarea = new List<int>() { };
            var result = new List<int>() { };

            var img = new Bitmap(bitmap);
            if (tune.ResizeDimension > 0)
            {
                var bestFitSize = Points.BestFit(new Size(bitmap.Width, bitmap.Height), new Size(tune.ResizeDimension, tune.ResizeDimension));
                img = CanvasFx.ResizeImage(bitmap, tune.BestFit ? bestFitSize : new Size(tune.ResizeDimension, tune.ResizeDimension));
                //img=GeneralFx.Contrast(img, 1);
            }

            for (var i = 0; i < tune.Division * tune.Division; i++)
            {
                filled.Add(0);
                allarea.Add(0);
                result.Add(0);
            }
            int zoneWidth = (int)Math.Round((double)img.Width / tune.Division, 0);
            int zoneHeight = (int)Math.Round((double)img.Height / tune.Division, 0);
            if (zoneWidth == 0 || zoneHeight == 0)
            {
                return null;
            }
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    int regionX = i / zoneWidth;
                    int regionY = j / zoneHeight;
                    if (regionX >= tune.Division) regionX = tune.Division - 1;
                    if (regionY >= tune.Division) regionY = tune.Division - 1;

                    allarea[regionY * tune.Division + regionX]++;
                    Color pixel = img.GetPixel(i, j);
                    if (pixel.Name != bgColor) //if not bg
                    {
                        filled[regionY * tune.Division + regionX]++;
                    }
                }
            }
            for (var i = 0; i < tune.Division * tune.Division; i++)
            {
                if (allarea[i] != 0)
                {

                    result[i] = (int)(((float)filled[i] / allarea[i]) * 100);
                }
                else
                {
                    result[i] = 0;
                }
            }

            return new IntVector(result);
        }

        public abstract int GuessCaptcha(Bitmap img, string bgColor);

    }
}
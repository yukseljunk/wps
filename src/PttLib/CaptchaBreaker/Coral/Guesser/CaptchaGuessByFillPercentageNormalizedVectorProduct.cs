using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PttLib.CaptchaBreaker.Coral.Guesser
{
    class CaptchaGuessByFillPercentageNormalizedVectorProduct:CaptchaGuess
    {

        private  double NormalizedProduct(IntVector fillPercentageRef, IntVector fillPercentage)
        {
            if (fillPercentageRef == null || fillPercentage == null || !fillPercentageRef.Any() || !fillPercentage.Any())
            {
                return -1;
            }
            return fillPercentageRef.NormalizedProduct(fillPercentage);
        }


        public  override int GuessCaptcha(Bitmap img, string bgColor)
        {
            var guesses = new List<int>();
            var tuneIndexor = 0;
            foreach (var tune in Tunes)
            {
                var selSegmentFillInfo = ReadBitmapFillPercentages(img, bgColor, tune);

                double maxTotal = 0.0d;
                var guess = -1;
                for (int numberIndexor = 0; numberIndexor < 10; numberIndexor++)
                {
                    var ratios = FillRatios[tuneIndexor][numberIndexor];
                    foreach (var ratio in ratios)
                    {
                        var fillPercentageDiff = NormalizedProduct(selSegmentFillInfo, ratio);
                        if (fillPercentageDiff != -1)
                        {
                            if (fillPercentageDiff > maxTotal)
                            {
                                maxTotal = fillPercentageDiff;
                                guess = numberIndexor;
                            }
                        }

                    }
                }
                guesses.Add(guess);
                tuneIndexor++;
            }

            return guesses.GroupBy(item => item).OrderByDescending(g => g.Count()).Select(g => g.Key).First();

        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PttLib.CaptchaBreaker.Coral.Guesser
{
    
    class CaptchaGuessByFillPercentageNeighborHood:CaptchaGuess
    {
        private int NeighborhoodDistance(IntVector fillPercentageRef, IntVector fillPercentage)
        {
            if (fillPercentageRef == null || fillPercentage == null || !fillPercentageRef.Any() || !fillPercentage.Any())
            {
                return -1;

            }
            if (fillPercentageRef.Count() != fillPercentage.Count())
            {
                return -1;
            }
            var total = 0;
            for (int pIndexor = 0; pIndexor < fillPercentageRef.Count(); pIndexor++)
            {
                var diff = Math.Abs(fillPercentageRef[pIndexor] - fillPercentage[pIndexor]);
                total += diff * diff;
            }
            return total;

        }
        

        public override int GuessCaptcha(Bitmap img, string bgColor)
        {
            var guesses= new List<int>();
            var tuneIndexor = 0;
            foreach (var tune in Tunes)
            {
                var selSegmentFillInfo = ReadBitmapFillPercentages(img, bgColor, tune);

                var minTotal = int.MaxValue;
                var guess = -1;
                for (int numberIndexor = 0; numberIndexor < 10; numberIndexor++)
                {
                    var ratios = FillRatios[tuneIndexor][numberIndexor];
                    foreach (var ratio in ratios)
                    {
                        var fillPercentageDiff = NeighborhoodDistance(selSegmentFillInfo, ratio);
                        if (fillPercentageDiff != -1)
                        {
                            if (fillPercentageDiff < minTotal)
                            {
                                minTotal = fillPercentageDiff;
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PttLib.CaptchaBreaker.Info
{
    class BitArray
    {
        /// <summary>
        /// return bool array, true for black, false for rest
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static bool[] ImageBitArray(Bitmap bitmap)
        {
            var result = new List<bool>();
            for (int jj = 0; jj < bitmap.Height; jj++)
            {
                for (int ii = 0; ii < bitmap.Width; ii++)
                {
                    Color pixelColor = bitmap.GetPixel(ii, jj);
                    if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0)//siyah ise 1
                    {
                        result.Add(true);
                    }
                    else
                    {
                        result.Add(false);
                    }

                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// show bit array in a nice form
        /// </summary>
        /// <param name="array"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string ImageBitArrayRepr(bool[] array, int width)
        {
            var result = new StringBuilder();
            var height = array.Length / width;
            for (int rowIndex = 0; rowIndex < height; rowIndex++)
            {
                var row = array.Skip(rowIndex * width).Take(width).ToList();
                foreach (var b in row)
                {
                    result.Append(b ? 1 : 0);
                }
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }

        public static void FalsifyBitVertical(bool[] image, int imageWidth, int offset, int falsingWidth)
        {
            var imageHeight = image.Length / imageWidth;
            for (int rowIndexor = 0; rowIndexor < imageHeight; rowIndexor++)
            {
                for (int eraseColumn = 0; eraseColumn < falsingWidth; eraseColumn++)
                {
                    image[rowIndexor * imageWidth + offset + eraseColumn] = false;

                }
            }
        }



        public static List<Point> FindBitBitmap(bool[] main, bool[] sub, int mainHeight, int subHeight)
        {
            var result = new List<Point>();
            int mainWidth = main.Length / mainHeight;
            int subWidth = sub.Length / subHeight;
            for (int iMain = 0; iMain < mainHeight; iMain++)
            {

                var currentMainRow = main.Skip(iMain * mainWidth).Take(mainWidth).ToList();
                var firstSubRow = sub.Skip(0).Take(subWidth).ToList();

                for (int frameIndex = 0; frameIndex < mainWidth - subWidth; frameIndex++)
                {
                    var frame = currentMainRow.Skip(frameIndex).Take(subWidth).ToList();
                    if (!BitsMatching(firstSubRow, frame)) continue;

                    var success = true;
                    for (int iSub = 1; iSub < subHeight; iSub++)
                    {
                        if ((iMain + iSub) * mainWidth + frameIndex >= main.Length)
                        {
                            success = false;
                            break;
                        }

                        var mainNextRow = main.Skip((iMain + iSub) * mainWidth + frameIndex).Take(subWidth).ToList();
                        var subNextRow = sub.Skip(iSub * subWidth).Take(subWidth).ToList();

                        success = BitsMatching(subNextRow, mainNextRow);
                        if (!success) break;
                    }
                    if (success)
                    {
                        result.Add(new Point(frameIndex, iMain + 1));
                    }
                }
            }

            return result;
        }

        private static bool BitsMatching(List<bool> item, List<bool> bg)
        {

            if (item.Count != bg.Count) return false;
            var success = true;
            for (int itemIndex = 0; itemIndex < item.Count; itemIndex++)
            {
                if (item[itemIndex] && bg[itemIndex] != item[itemIndex])
                {
                    success = false;
                    break;
                }
            }
            return success;
        }

    }
}
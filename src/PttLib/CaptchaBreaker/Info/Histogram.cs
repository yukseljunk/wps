using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PttLib.CaptchaBreaker.Info
{
    class Histogram
    {
        public static Dictionary<string, int> Palette(Bitmap bitmap)
        {
            var palette = new Dictionary<string, int>();
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixel = bitmap.GetPixel(i, j);
                    if (palette.ContainsKey(pixel.Name))
                    {
                        palette[pixel.Name]++;
                    }
                    else
                    {
                        palette[pixel.Name] = 1;
                    }

                }
            }

            return palette;
        }
        public static string BgColor(Bitmap bitmap)
        {
            var palette = Palette(bitmap);
            if (!palette.Any())
            {
                return null;
            }
            var ordPalette = palette.OrderByDescending(p => p.Value);
            foreach (var pal in ordPalette)
            {
                return pal.Key;
            }
            return null;
        }
        public static List<string> Top10Colors(Bitmap bitmap)
        {
            var palette = Palette(bitmap);
            var ordPalette = palette.OrderByDescending(p => p.Value);
            var indexor = 1;
            var top10Colors = new List<string>();
            foreach (var pal in ordPalette)
            {
                if (indexor < 11)
                {
                    top10Colors.Add(pal.Key);
                }
                indexor++;
            }
            return top10Colors;
        }
    }
}

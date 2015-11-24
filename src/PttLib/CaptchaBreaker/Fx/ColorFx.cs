using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using PttLib.CaptchaBreaker.Info;
using System.Drawing;

namespace PttLib.CaptchaBreaker.Fx
{
    internal class ColorFx
    {
        public static Bitmap LeaveTop10Colors(Bitmap bitmap, string bgColor)
        {
            var top10Colors = Histogram.Top10Colors(bitmap);
            return GeneralFx.MakePixelsBg(bitmap, bgColor, (pixel) => (top10Colors.Contains(pixel.Name)));
        }


        public static Bitmap RemoveColor(Bitmap bitmap, string bgColor, string colorToRemove)
        {
            return GeneralFx.MakePixelsBg(bitmap, bgColor, (pixel) => colorToRemove == pixel.Name);
        }

        public static Bitmap RemoveTransparentColors(Bitmap bitmap, string bgColor)
        {
            return GeneralFx.MakePixelsBg(bitmap, bgColor, (pixel) => pixel.A != 255);
        }

        public static Bitmap ConvertTo1Bit(Bitmap input)
        {
            var masks = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
            var output = new Bitmap(input.Width, input.Height, PixelFormat.Format1bppIndexed);
            var data = new sbyte[input.Width, input.Height];
            var inputData = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                var scanLine = inputData.Scan0;
                var line = new byte[inputData.Stride];
                for (var y = 0; y < inputData.Height; y++, scanLine += inputData.Stride)
                {
                    Marshal.Copy(scanLine, line, 0, line.Length);
                    for (var x = 0; x < input.Width; x++)
                    {
                        data[x, y] = (sbyte)(64 * (GetGreyLevel(line[x * 3 + 2], line[x * 3 + 1], line[x * 3 + 0]) - 0.5));
                    }
                }
            }
            finally
            {
                input.UnlockBits(inputData);
            }
            var outputData = output.LockBits(new Rectangle(0, 0, output.Width, output.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
            try
            {
                var scanLine = outputData.Scan0;
                for (var y = 0; y < outputData.Height; y++, scanLine += outputData.Stride)
                {
                    var line = new byte[outputData.Stride];
                    for (var x = 0; x < input.Width; x++)
                    {
                        var j = data[x, y] > 0;
                        if (j) line[x / 8] |= masks[x % 8];
                        var error = (sbyte)(data[x, y] - (j ? 32 : -32));
                        if (x < input.Width - 1) data[x + 1, y] += (sbyte)(7 * error / 16);
                        if (y < input.Height - 1)
                        {
                            if (x > 0) data[x - 1, y + 1] += (sbyte)(3 * error / 16);
                            data[x, y + 1] += (sbyte)(5 * error / 16);
                            if (x < input.Width - 1) data[x + 1, y + 1] += (sbyte)(1 * error / 16);
                        }
                    }
                    Marshal.Copy(line, 0, scanLine, outputData.Stride);
                }
            }
            finally
            {
                output.UnlockBits(outputData);
            }
            return output;
        }

        public static double GetGreyLevel(byte r, byte g, byte b)
        {
            return (r * 0.299 + g * 0.587 + b * 0.114) / 255;
        }

        private static bool ColorMatch(Color a, Color b)
        {
            return (a.ToArgb() & 0xffffff) == (b.ToArgb() & 0xffffff);
        }

        public static Bitmap FloodFill(Bitmap bitmap, Point pt, Color targetColor, Color replacementColor, out List<Point> replacements)
        {
            replacements = new List<Point>();
            var bmp = (Bitmap) bitmap.Clone();
            Queue<Point> q = new Queue<Point>();
            q.Enqueue(pt);
            while (q.Count > 0)
            {
                Point n = q.Dequeue();
                if (!ColorMatch(bmp.GetPixel(n.X, n.Y), targetColor))
                    continue;
                Point w = n, e = new Point(n.X + 1, n.Y);
                while ((w.X > 0) && ColorMatch(bmp.GetPixel(w.X, w.Y), targetColor))
                {
                    bmp.SetPixel(w.X, w.Y, replacementColor);
                    replacements.Add(new Point(w.X, w.Y));
                    if ((w.Y > 0) && ColorMatch(bmp.GetPixel(w.X, w.Y - 1), targetColor))
                        q.Enqueue(new Point(w.X, w.Y - 1));
                    if ((w.Y < bmp.Height - 1) && ColorMatch(bmp.GetPixel(w.X, w.Y + 1), targetColor))
                        q.Enqueue(new Point(w.X, w.Y + 1));
                    w.X--;
                }
                while ((e.X < bmp.Width - 1) && ColorMatch(bmp.GetPixel(e.X, e.Y), targetColor))
                {
                    bmp.SetPixel(e.X, e.Y, replacementColor);
                    replacements.Add(new Point(e.X, e.Y));
                    if ((e.Y > 0) && ColorMatch(bmp.GetPixel(e.X, e.Y - 1), targetColor))
                        q.Enqueue(new Point(e.X, e.Y - 1));
                    if ((e.Y < bmp.Height - 1) && ColorMatch(bmp.GetPixel(e.X, e.Y + 1), targetColor))
                        q.Enqueue(new Point(e.X, e.Y + 1));
                    e.X++;
                }
            }
            return bmp;
        }


        /// <summary>
        /// extended floodfill, returns also the points changed by floodfill, used for monochromatic image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="allReplacedPoints"></param>
        /// <returns></returns>
        public static Bitmap FloodFill(Bitmap bitmap, out List<List<Point>> allReplacedPoints)
        {
            allReplacedPoints = new List<List<Point>>();
            var image = (Bitmap)bitmap.Clone();
            var someColors = new List<Color>()
                {
                    Color.Green,
                    Color.Blue,
                    Color.Red,
                    Color.Orange,
                    Color.Yellow,
                    Color.Aqua,
                    Color.Fuchsia,
                    Color.Pink,
                    Color.Lime,
                    Color.PowderBlue,
                    Color.SaddleBrown
                };
            var col = someColors[0];

            List<Point> replacedPoints;
            var colorIndex = 0;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    image = FloodFill(image, new Point(i, j), Color.White, col, out replacedPoints);

                    if (!replacedPoints.Any())
                    {
                        //no white
                    }
                    else if (replacedPoints.Count() < 20)
                    {
                        //most probably line or disconnected piece
                        colorIndex++;
                        col = someColors[colorIndex % someColors.Count];
                        allReplacedPoints.Add(replacedPoints);
                    }
                    else
                    {
                        colorIndex++;
                        col = someColors[colorIndex % someColors.Count];
                        allReplacedPoints.Add(replacedPoints);
                    }
                }
            }
            return image;
        }
    }
}

using System.Drawing;
using PttLib.CaptchaBreaker.Info;

namespace PttLib.CaptchaBreaker.Fx
{
    class NoiseFx
    {
        public static Bitmap RemoveDots(Bitmap bitmap, string bgColor)
        {
            var image = (Bitmap) bitmap.Clone();
            var edgePoint = new Point(image.Width - 1, image.Height - 1);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    if (pixel.Name != bgColor)//if not bg
                    {
                        var neighbors = Points.Neighbors(new Point(i, j), edgePoint);
                        var allNeighborsBg = true;
                        foreach (var neighbor in neighbors)
                        {
                            if (image.GetPixel(neighbor.X, neighbor.Y).Name != bgColor)
                            {
                                allNeighborsBg = false;
                                break;
                            }
                        }
                        if (allNeighborsBg)
                        {
                            image.SetPixel(i, j, ColorTranslator.FromHtml("#" + bgColor));

                        }
                    }

                }
            }
            return image;
        }
    }
}

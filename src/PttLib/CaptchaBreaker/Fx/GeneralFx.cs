using System.Drawing;

namespace PttLib.CaptchaBreaker.Fx
{
    class GeneralFx
    {
        public delegate bool PixelIsIn(Color pixelColor);

        public static Bitmap MakePixelsBg(Bitmap bitmap, string bgColor, PixelIsIn pixelIsIn)
        {
            var img = (Bitmap)bitmap.Clone();
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (pixelIsIn(pixel))
                    {
                        img.SetPixel(i, j, ColorTranslator.FromHtml("#" + bgColor));
                    }
                }
            }
            return img;
        }

    }
}

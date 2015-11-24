using System.Drawing;

namespace PttLib.CaptchaBreaker.Fx
{
    class NegateFx
    {
        public static Bitmap Negate(Bitmap bitmap)
        {
            var sourceimage = (Bitmap)bitmap.Clone();

            Color c;
            for (int i = 0; i < sourceimage.Width; i++)
            {
                for (int j = 0; j < sourceimage.Height; j++)
                {
                    c = sourceimage.GetPixel(i, j);
                    c = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                    sourceimage.SetPixel(i, j, c);
                }
            }

            return sourceimage;
        }
    }
}

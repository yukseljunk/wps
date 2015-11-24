using System.Drawing;

namespace PttLib.CaptchaBreaker.Fx
{
    class CanvasFx
    {
        /// <summary>
        /// resize canvas, put bitmap into center
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap EnlargeCanvas(Bitmap bitmap, int width, int height, string bgColorToFill)
        {
            var img = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    img.SetPixel(i, j, ColorTranslator.FromHtml("#" + bgColorToFill));

                }
            }

            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(bitmap,
                   new Rectangle((width - bitmap.Width) / 2, (height - bitmap.Height) / 2, bitmap.Width, bitmap.Height),
                                 new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                 GraphicsUnit.Pixel);
            }

            return img;
        }

        public static Bitmap Crop(Bitmap bitmap, Rectangle cropArea)
        {
            Bitmap target = new Bitmap(cropArea.Width, cropArea.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(bitmap,
                    new Rectangle(0, 0, target.Width, target.Height),
                                 cropArea,
                                 GraphicsUnit.Pixel);
            }
            return target;
        }

        public static Bitmap ResizeImage(Image imgToResize, Size size)
        {
            return (new Bitmap(imgToResize, size));
        }
    }
}

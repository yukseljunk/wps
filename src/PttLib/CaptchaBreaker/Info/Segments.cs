using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PttLib.CaptchaBreaker.Info
{
    class Segments
    {
        public static List<Bitmap> GetSegments(Bitmap img, string bgColor, bool dummy)
        {
            var result = new List<Bitmap>();
            var segments = GetSegments(img, bgColor);
            if (!segments.Any())
            {
                return null;
            }
            foreach (var rectangle in segments)
            {
                var target = new Bitmap(rectangle.Width, rectangle.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(img,
                        new Rectangle(0, 0, target.Width, target.Height),
                                     rectangle,
                                     GraphicsUnit.Pixel);
                }
                result.Add(target);
            }
           
            return result;
        }

        /// <summary>
        /// returns rectangle segments of bitmap
        /// </summary>
        /// <param name="img"></param>
        /// <param name="bgColor"> </param>
        /// <returns></returns>
        public static List<Rectangle> GetSegments(Bitmap img, string bgColor)
        {
            var segments = new List<Rectangle>();

            var verticalSegments = VerticalSegments(img, bgColor);

            //horizontal partial segments
            foreach (var rectangle in verticalSegments)
            {
                var rectangleY = CalculateRectangleTop(img, bgColor, rectangle);
                var rectangleH = CalculateRectangleBottom(img, bgColor, rectangle);
                segments.Add(new Rectangle(rectangle.X, rectangleY, rectangle.Width, rectangleH - rectangleY));
            }
            return segments;
        }

        private static int CalculateRectangleBottom(Bitmap img, string bgColor, Rectangle rectangle)
        {
            int rectangleH = 0;
            for (int j = rectangle.Bottom - 1; j >= rectangle.Y; j--)
            {
                var fullRowBg = true;
                for (int i = rectangle.X; i < rectangle.Right; i++)
                {
                    Color pixel = img.GetPixel(i, j);


                    if (pixel.Name != bgColor) //if not bg
                    {
                        fullRowBg = false;
                        break;
                    }
                }
                if (!fullRowBg)
                {
                    rectangleH = j + 1;
                    //burasi rectangle in bottom pozisyonu 
                    break;
                }
            }
            return rectangleH;
        }

        private static int CalculateRectangleTop(Bitmap img, string bgColor, Rectangle rectangle)
        {
            int rectangleY = 0;
            for (int j = rectangle.Y; j < rectangle.Bottom; j++)
            {
                var fullRowBg = true;
                for (int i = rectangle.X; i < rectangle.Right; i++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (pixel.Name != bgColor) //if not bg
                    {
                        fullRowBg = false;
                        break;
                    }
                }
                if (!fullRowBg)
                {
                    rectangleY = j - 1;
                    //burasi rectangle in top pozisyonu yani Y
                    break;
                }
            }
            return rectangleY;
        }

        private static List<Rectangle> VerticalSegments(Bitmap img, string bgColor)
        {
            var verticalSegments = new List<Rectangle>();
            var lastLineFullBg = true;
            var lastRectStart = 0;
            for (int i = 0; i < img.Width; i++)
            {
                var fullLineBg = true;
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    if (pixel.Name != bgColor) //if not bg
                    {
                        fullLineBg = false;
                        break;
                    }
                }
/*                if (fullLineBg)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                    //    img.SetPixel(i, j, Color.Red);
                    }
                }
                */
                if (!fullLineBg && lastLineFullBg)
                {
                    lastRectStart = i;
                }
                if (fullLineBg && !lastLineFullBg)
                {
                    verticalSegments.Add(new Rectangle(lastRectStart, 0, i - lastRectStart, img.Height));
                }
                lastLineFullBg = fullLineBg;
            }
            return verticalSegments;
        } 

    }
}

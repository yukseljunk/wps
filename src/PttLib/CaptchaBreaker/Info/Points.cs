using System;
using System.Collections.Generic;
using System.Drawing;

namespace PttLib.CaptchaBreaker.Info
{
    internal class Points
    {
        public static List<Point> Neighbors(Point refPoint, Point edgePoint)
        {
            if (edgePoint.X < 0 || edgePoint.Y < 0) throw new ArgumentException("not valid edgepoint");
            if (refPoint.X < 0 || refPoint.Y < 0) throw new ArgumentException("not valid refpoint");
            if (refPoint.X > edgePoint.X || refPoint.Y > edgePoint.Y) throw new ArgumentException("not valid refpoint");

            var result = new List<Point>();
            //right
            if (refPoint.X < edgePoint.X) result.Add(new Point(refPoint.X + 1, refPoint.Y));
            //left
            if (refPoint.X > 0) result.Add(new Point(refPoint.X - 1, refPoint.Y));
            //top
            if (refPoint.Y > 0) result.Add(new Point(refPoint.X, refPoint.Y - 1));
            //bottom
            if (refPoint.Y < edgePoint.Y) result.Add(new Point(refPoint.X, refPoint.Y + 1));

            //rightbottom
            if (refPoint.X < edgePoint.X && refPoint.Y < edgePoint.Y)
                result.Add(new Point(refPoint.X + 1, refPoint.Y + 1));

            //leftbottom
            if (refPoint.X > 0 && refPoint.Y < edgePoint.Y) result.Add(new Point(refPoint.X - 1, refPoint.Y + 1));

            //lefttop
            if (refPoint.X > 0 && refPoint.Y > 0) result.Add(new Point(refPoint.X - 1, refPoint.Y - 1));

            //righttop
            if (refPoint.X < edgePoint.X && refPoint.Y > 0) result.Add(new Point(refPoint.X + 1, refPoint.Y - 1));

            return result;
        }

        public static Size BestFit(Size image, Size boundingBox)
        {
            double widthScale = 0, heightScale = 0;
            if (image.Width != 0)
                widthScale = (double) boundingBox.Width/(double) image.Width;
            if (image.Height != 0)
                heightScale = (double) boundingBox.Height/(double) image.Height;

            double scale = Math.Min(widthScale, heightScale);

            Size result = new Size((int) (image.Width*scale),
                                   (int) (image.Height*scale));
            return result;
        }

    }
}

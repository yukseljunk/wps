using System.Drawing;

namespace PttLib.CaptchaBreaker.Fx
{

    class ChannelFx
    {

   
        public static Bitmap RemoveRed(Bitmap bitmap, string bgColor)
        {
            return GeneralFx.MakePixelsBg(bitmap, bgColor, (pixelColor) => (pixelColor.R > pixelColor.G && pixelColor.R > pixelColor.B));
        }

        public static Bitmap RemoveGreen(Bitmap bitmap, string bgColor)
        {
            return GeneralFx.MakePixelsBg(bitmap, bgColor, (pixel) => (pixel.G > pixel.R && pixel.G >= pixel.B));
        }
        public static Bitmap RemoveBlue(Bitmap bitmap, string bgColor)
        {
            return GeneralFx.MakePixelsBg(bitmap, bgColor, (pixel) => (pixel.B >= pixel.G && pixel.B > pixel.R));
        }          

    }
}

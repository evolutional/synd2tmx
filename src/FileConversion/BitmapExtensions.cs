using System.Drawing;

namespace FileConversion
{
    static class BitmapExtensions
    {
        public static void BlitIntoBitmap(this Bitmap Target, Bitmap Source, int x, int y)
        {
            for (var yy = 0; yy < Source.Height; ++yy)
            {
                for (var xx = 0; xx < Source.Width; ++xx)
                {
                    Target.SetPixel(x + xx, y + yy, Source.GetPixel(xx, yy));
                }
            }
        }
    }
}

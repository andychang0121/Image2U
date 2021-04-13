using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Image2U.Service.Helper
{
    public class ImageHelper
    {
        internal static byte[] ImageToBytes(Image image, ImageFormat imageFormat)
        {
            byte[] bytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap bitmap = new Bitmap(image))
                {
                    bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    bytes = new byte[ms.Length];
                    ms.Read(bytes, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return bytes;
        }

        internal static double GetRatio(bool isPortait, int assignWidth, int width, int height)
        {
            if (assignWidth == width) return 1;

            return assignWidth / (double)width;
        }
        //=> assignWidth == width ?
        //    1 :
        //    isPortait ? assignWidth / (double)height : assignWidth / (double)width;
    }
}

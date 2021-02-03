using Image2U.Web.Enum;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Image2U.Web.Helper
{
    public static partial class ImageHelper
    {
        public static byte[] ImageToByteArray(this Image image, ImageFormat imageFormat)
        {
            byte[] bytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                //建立副本
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

        public static Bitmap Resize(this Image image, double ratio, ImageDirection direction, int ecWidth, int ecHeight)
        {
            if (direction == ImageDirection.Portait)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipX);
            }

            Bitmap bitmapRs = Resize(image, ratio, ecWidth, ecHeight);

            return bitmapRs;
        }

        public static Bitmap Resize(this Image image, double ratio, int ecWidth, int ecHeight)
        {
            Models.Image.ImageProfile originalImageProfile = new Models.Image.ImageProfile(image);

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(originalImageProfile.Width * ratio);
            int newHeight = Convert.ToInt32(originalImageProfile.Height * ratio);

            if ((int)ratio == 1)
            {
                newWidth = ecWidth;
                newHeight = ecHeight;
            }

            // -----
            Bitmap thumbnail = new Bitmap(newWidth, newHeight);

            thumbnail.SetResolution(72, 72);

            Graphics graphic = Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            graphic.Clear(Color.Transparent);
            graphic.DrawImage(image, 0, 0, newWidth, newHeight);

            return thumbnail;
        }
    }
}

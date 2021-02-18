using Image2U.Web.Enum;
using Image2U.Web.Models.Image;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Image2U.Web.Helper
{
    public static class ImageFileExtension
    {
        public static string GetZipFileName(this ImageFile imageFile,string folderName, int w, int h)
            => $"{folderName}\\{imageFile.FileName}-{w}x{h}.{imageFile.Ext}";

        public static string GetZipFileName(this string fileName, string ext, int w, int h)
            => $"{fileName}-{w}x{h}.{ext}";

        public static byte[] Resize(this ImageFile imageFile, int w, int h, ImageFormat imageFormat)
        {
            using (Bitmap image = new Bitmap(imageFile.Stream))
            {
                double ratio = imageFile.GetRatio(w, image.Width, image.Height);

                Bitmap newImage = image.Resize(ratio, imageFile.Direction, w, h);

                byte[] bytes = newImage.ImageToBytes(imageFormat);

                return bytes;
            }
        }

        private static byte[] ImageToBytes(this System.Drawing.Image image, ImageFormat imageFormat)
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

        private static double GetRatio(this ImageFile imageFile, int limitWidth, int width, int height)
            => limitWidth == width ?
                1 :
                imageFile.Direction == ImageDirection.Portait ? limitWidth / (double)height : limitWidth / (double)width;
    }
    public static partial class ImageHelper
    {
        public static Bitmap CustomResize(this Image image, double ratio, ImageDirection direction, int width, int height, bool isCustom)
        {
            if (direction == ImageDirection.Portait)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipX);
            }

            Bitmap bitmapRs = CustomResize(image, ratio, width, height, isCustom: isCustom);

            return bitmapRs;
        }

        private static Bitmap CustomResize(this Image image, double ratio, int width, int height, float xDpi = 72, float yDpi = 72, bool isCustom = false)
        {
            ImageProfile originalImageProfile = new ImageProfile(image);

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(originalImageProfile.Width * ratio);
            int newHeight = Convert.ToInt32(originalImageProfile.Height * ratio);

            if ((int)ratio == 1)
            {
                newWidth = width;
                newHeight = height;
            }

            int y = newHeight - height;

            Rectangle srcRect = new Rectangle(0, y, newWidth, height);

            Rectangle targetRect = new Rectangle(0, 0, newWidth, newHeight);

            Bitmap srcImage = (Bitmap)image;
            Bitmap thumbnail = new Bitmap(newWidth, newHeight);

            thumbnail.SetResolution(xDpi, yDpi);

            Graphics graphic = Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            graphic.Clear(Color.Transparent);

            graphic.DrawImage(srcImage, targetRect, srcRect, GraphicsUnit.Pixel);

            return thumbnail;
        }

        private static Bitmap Resize(this Image image, double ratio, ImageDirection direction, int width, int height, bool isCustom)
        {
            if (direction == ImageDirection.Portait)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipX);
            }

            if (isCustom) return CustomResize(image, ratio, width, height);

            Bitmap bitmapRs = Resize(image, ratio, width, height);

            return bitmapRs;
        }

        public static Bitmap Resize(this Image image, double ratio, ImageDirection direction, int width, int height)
        {
            if (direction == ImageDirection.Portait)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipX);
            }

            Bitmap bitmapRs = Resize(image, ratio, width, height);

            return bitmapRs;
        }

        private static Bitmap Resize(this Image image, double ratio, int width, int height, float xDpi = 72, float yDpi = 72)
        {
            ImageProfile originalImageProfile = new ImageProfile(image);

            // now we can get the new height and width
            int newWidth = Convert.ToInt32(originalImageProfile.Width * ratio);
            int newHeight = Convert.ToInt32(originalImageProfile.Height * ratio);

            if ((int)ratio == 1)
            {
                newWidth = width;
                newHeight = height;
            }

            // -----
            Bitmap thumbnail = new Bitmap(newWidth, newHeight);

            thumbnail.SetResolution(xDpi, yDpi);

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

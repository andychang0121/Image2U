using System;
using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Image2U.Service.Enum;
using Image2U.Service.Models.Image;
using Image2U.Web.Enum;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        public byte[] Resize(ImageFile imageFile, int reWidth, int reHeight, ImageFormat imageFormat)
        {
            using (Bitmap image = new Bitmap(imageFile.Stream))
            {
                double ratio = GetRatio(imageFile.IsPortait, reWidth, image.Width, image.Height);

                Bitmap newImage = Resize(image, ratio, imageFile.IsPortait, reWidth, reHeight);

                byte[] bytes = ImageToBytes(newImage, imageFormat);

                return bytes;
            }
        }

        public Bitmap Resize(Image image, double ratio, ImageDirection direction, int width, int height)
        {
            if (direction == ImageDirection.Portait)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipX);
            }

            Bitmap bitmapRs = Resize(image, ratio, width, height);

            return bitmapRs;
        }

        public Bitmap Resize(Image image, double ratio, bool isPortait, int width, int height)
        {
            if (isPortait)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipX);
            }

            Bitmap bitmapRs = Resize(image, ratio, width, height);

            return bitmapRs;
        }

        private Bitmap Resize(Image image, double ratio, int width, int height, float xDpi = 72, float yDpi = 72)
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

        private byte[] ImageToBytes(Image image, ImageFormat imageFormat)
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

        private static double GetRatio(ImageFile imageFile, int limitWidth, int width, int height)
            => limitWidth == width ?
                1 :
                imageFile.Direction == ImageDirection.Portait ? limitWidth / (double)height : limitWidth / (double)width;

        private static double GetRatio(bool isPortait, int limitWidth, int width, int height)
            => limitWidth == width ?
                1 :
                isPortait ? limitWidth / (double)height : limitWidth / (double)width;
    }
}
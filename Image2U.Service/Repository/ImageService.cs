using Image2U.Service.Helper;
using Image2U.Service.Interface;
using Image2U.Service.Models.Image;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Image2U.Service.Repository
{
    public class ImageService : IImageService
    {
        public async Task<byte[]> GetImageBytesAsync(Stream stream, string fileName, bool isPortait, int width, int height, string ecName)
        {
            byte[] bytes = await ResizeAsync(stream, isPortait, width, height, ImageFormat.Jpeg);

            return bytes;
        }

        private async Task<byte[]> ResizeAsync(Stream stream, bool isPortait, int reWidth, int reHeight, ImageFormat imageFormat)
        {
            using (Bitmap image = new Bitmap(stream))
            {
                double ratio = ImageHelper.GetRatio(isPortait, reWidth, image.Width, image.Height);

                Bitmap newImage = await ResizeAsync(image, ratio, isPortait, reWidth, reHeight);

                byte[] bytes = ImageHelper.ImageToBytes(newImage, imageFormat);

                return bytes;
            }
        }

        private async Task<Bitmap> ResizeAsync(Image image, double ratio, bool isPortait, int width, int height)
        {
            Bitmap bitmapRs = await ResizeAsync(image, ratio, width, height);

            return bitmapRs;
        }

        private Bitmap ResizeAsync(Image src, int width, int height)
        {
            Rectangle cropRect = new Rectangle(0, 0, width, height);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            Graphics graphics = Graphics.FromImage(target);

            using (Graphics g = graphics)
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                    cropRect,
                    GraphicsUnit.Pixel);
            }
            return target;
        }

        private async Task<Bitmap> ResizeAsync(Image image, double ratio, int width, int height, float xDpi = 72.0f, float yDpi = 72.0f)
        {
            return await Task.Run(() =>
            {
                ImageProfile originalImageProfile = new ImageProfile(image);

                int newWidth = Convert.ToInt32(originalImageProfile.Width * ratio);
                int newHeight = Convert.ToInt32(originalImageProfile.Height * ratio);

                if (Math.Abs(ratio - 1) == 0)
                {
                    newWidth = originalImageProfile.Width;
                    newHeight = originalImageProfile.Height;
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
                graphic.Dispose();

                //Bitmap rs = crop(thumbnail, thumbnail.Width, height);
                //return rs;
                return thumbnail;
            });
        }
    }
}

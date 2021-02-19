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
        public async Task<byte[]> Resize(ImageFile imageFile, int reWidth, int reHeight, ImageFormat imageFormat)
        {
            using (Bitmap image = new Bitmap(imageFile.Stream))
            {
                double ratio = ImageHelper.GetRatio(imageFile.IsPortait, reWidth, image.Width, image.Height);

                Bitmap newImage = await Resize(image, ratio, imageFile.IsPortait, reWidth, reHeight);

                byte[] bytes = ImageHelper.ImageToBytes(newImage, imageFormat);

                return bytes;
            }
        }

        public async Task<byte[]> ResizeAsync(ImageFile imageFile, int reWidth, int reHeight, ImageFormat imageFormat)
        {
            using (Bitmap image = new Bitmap(imageFile.Stream))
            {
                double ratio = ImageHelper.GetRatio(imageFile.IsPortait, reWidth, image.Width, image.Height);

                Bitmap newImage = await Resize(image, ratio, imageFile.IsPortait, reWidth, reHeight);

                byte[] bytes = ImageHelper.ImageToBytes(newImage, imageFormat);

                return bytes;
            }
        }

        public async Task<Bitmap> Resize(Image image, double ratio, bool isPortait, int width, int height)
        {
            if (isPortait)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipX);
            }

            Bitmap bitmapRs = await Resize(image, ratio, width, height);

            return bitmapRs;
        }

        /// <summary>
        /// Todo:
        /// </summary>
        /// <param name="image"></param>
        /// <param name="ratio"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="xDpi"></param>
        /// <param name="yDpi"></param>
        /// <returns></returns>
        private async Task<Bitmap> Resize(Image image, double ratio, int width, int height, float xDpi = 72, float yDpi = 72)
        {
            return await Task.Run(() =>
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
            });
        }

        public async Task<byte[]> GetImageBytesAsync(Stream stream, string fileName, bool isPortait, int width, int height, string ecName)
        {
            ImageFile imageFile = new ImageFile(stream, fileName, isPortait);

            byte[] bytes = await Resize(imageFile, width, height, ImageFormat.Jpeg);

            return bytes;
        }
    }
}

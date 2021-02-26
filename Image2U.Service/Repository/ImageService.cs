using System;
using Image2U.Service.Helper;
using Image2U.Service.Interface;
using Image2U.Service.Models;
using Image2U.Service.Models.Image;
using Image2U.Web.Models.Image;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Image2U.Service.Enum;

namespace Image2U.Service.Repository
{
    public class ImageService : IImageService
    {
        public async Task<Dictionary<string, ImageOutput>> SizingAsync(RequestData requestData, Dictionary<string, ImageOutput> dict)
        {
            Dictionary<string, ImageOutput> refDict = dict;

            if (requestData.IsCustomSize)
            {
                refDict = new Dictionary<string, ImageOutput>
                {
                    {"自定義尺寸",new ImageOutput {
                        Width = requestData.CustomWidth.Value,
                        MaxHeight = requestData.CustomHeight.Value,
                        DPI = 72
                    }},
                };
            }

            Dictionary<string, ImageOutput> rs = await ResizingAsync(requestData, refDict);

            return rs;
        }

        private async Task<Dictionary<string, ImageOutput>> ResizingAsync(RequestData requestData, Dictionary<string, ImageOutput> ecProfile)
        {
            foreach (KeyValuePair<string, ImageOutput> ec in ecProfile)
            {
                ImageOutput ecSetting = ec.Value;

                bool isPortait = requestData.IsPortsait();

                Stream stream = requestData.Base64.GetStream();

                ImageFile imageFile = new ImageFile(stream, requestData.FileName, isPortait);

                byte[] bytesRs = await Task.Run(() =>
                    Resize(imageFile, ecSetting.Width, ecSetting.MaxHeight, ImageFormat.Jpeg));

                //ZipData zipData =
                //    await GetZipAsync(stream, requestData.FileName, isPortait, ecSetting.Width, ecSetting.MaxHeight, ec.Key);

                ec.Value.Bytes = bytesRs;
            }

            return ecProfile;
        }

        public byte[] Resize(ImageFile imageFile, int w, int h, ImageFormat imageFormat)
        {
            using (Bitmap image = new Bitmap(imageFile.Stream))
            {
                double ratio = GetRatio(imageFile, w, image.Width, image.Height);

                Bitmap newImage = Resize(image, ratio, imageFile.Direction, w, h);

                byte[] bytes = ImageToBytes(newImage, imageFormat);

                return bytes;
            }
        }

        private Bitmap Resize(Image image, double ratio, ImageDirection direction, int width, int height)
        {
            if (direction == ImageDirection.Portait)
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

        private static double GetRatio(ImageFile imageFile, int limitWidth, int width, int height)
            => limitWidth == width ?
                1 :
                imageFile.Direction == ImageDirection.Portait ? limitWidth / (double)height : limitWidth / (double)width;

        private static byte[] ImageToBytes(Image image, ImageFormat imageFormat)
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
    }
}

using Image2U.Service.Enum;
using Image2U.Service.Models.Image;
using ImageMagick;
using System;
using System.Threading.Tasks;

namespace Image2U.Web.Helper
{
    public static partial class ImageHelper
    {
        public static async Task<ImageResult> Resize(this MagickImage image, double ratio, ImageDirection direction)
        {
            if (direction == ImageDirection.Portait)
            {
                image.Rotate(90);
            }

            int height = image.Height;
            int width = image.Width;

            ImageResult resizeRs = await Task.Run(() => Resize(image, width, height, ratio));

            return resizeRs;
        }

        public static ImageResult Resize(this MagickImage originalImage, int originalWidth, int originalHeight, double ratio)
        {
            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);
            // -----
            using (MagickImage image = new MagickImage(originalImage))
            {
                image.Format = MagickFormat.Jpeg;
                image.Resize(newWidth, newHeight);
                image.Strip();
                byte[] bytes = image.ToByteArray();
                ImageResult rs = new ImageResult
                {
                    Width = newWidth,
                    Height = newHeight,
                    Bytes = bytes
                };

                return rs;
            }
        }
    }
}

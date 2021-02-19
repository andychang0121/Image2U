using Image2U.Service.Models.Image;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Image2U.Service.Interface
{
    public interface IImageService
    {
        Task<byte[]> GetImageBytesAsync(Stream stream, string fileName, bool isPortait, int width, int height,
            string ecName);

        Task<byte[]> Resize(ImageFile imageFile, int reWidth, int reHeight, ImageFormat imageFormat);

        Task<byte[]> ResizeAsync(ImageFile imageFile, int reWidth, int reHeight, ImageFormat imageFormat);
    }
}

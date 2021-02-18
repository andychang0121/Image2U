using Image2U.Service.Models.Image;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Image2U.Service.Interface
{
    public interface IImageService
    {
        byte[] Resize(ImageFile imageFile, int reWidth, int reHeight, ImageFormat imageFormat);

        Task<byte[]> ResizeAsync(ImageFile imageFile, int reWidth, int reHeight, ImageFormat imageFormat);
    }
}

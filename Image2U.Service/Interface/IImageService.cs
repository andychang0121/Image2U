using System.IO;
using System.Threading.Tasks;

namespace Image2U.Service.Interface
{
    public interface IImageService
    {
        Task<byte[]> GetImageBytesAsync(Stream stream, string fileName, bool isPortait, int width, int height,
            string ecName);
    }
}

using Image2U.Service.Models;
using Image2U.Service.Models.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Image2U.Service.Interface
{
    public interface IImageService
    {
        Task<Dictionary<string, ImageOutput>> SizingAsync(RequestData requestData, Dictionary<string, ImageOutput> dict);
    }
}

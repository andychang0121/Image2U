using Image2U.Service.Models;
using Image2U.Service.Models.Image;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Image2U.Service.ImageHandler
{
    public interface IImageHandler
    {
        Task<ResponseData> ConvertAsync(RequestData requestData, Dictionary<string, ImageOutput> dict);
    }
}

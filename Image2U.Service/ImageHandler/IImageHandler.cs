using Image2U.Service.Models;
using Image2U.Service.Models.Image;
using System.Collections.Generic;
using System.Threading.Tasks;
using Image2U.Service.Models.Process;

namespace Image2U.Service.ImageHandler
{
    public interface IImageHandler
    {
        Task<ProcessData> ConvertAsync(RequestData requestData, Dictionary<string, ImageOutput> dict = null);
    }
}

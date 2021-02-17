using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image2U.Service.Models;
using Image2U.Service.Models.Image;

namespace Image2U.Service.Interface
{
    public interface IImageService
    {
        Task<Dictionary<string, ImageOutput>> SizingAsync(RequestData requestData, Dictionary<string, ImageOutput> dict);
    }
}

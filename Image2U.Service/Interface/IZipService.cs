using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image2U.Service.Models;
using Image2U.Service.Models.Image;

namespace Image2U.Service.Interface
{
    public interface IZipService
    {
        Task<IEnumerable<ZipData>> GetZip(RequestData requestData, Dictionary<string, ImageOutput> ecProfile);
    }
}

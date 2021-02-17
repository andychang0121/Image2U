using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image2U.Service.Interface;
using Image2U.Service.Models;
using Image2U.Service.Models.Image;

namespace Image2U.Service.Repository
{
    public class ZipService : IZipService
    {
        public Task<IEnumerable<ZipData>> GetZip(RequestData requestData, Dictionary<string, ImageOutput> ecProfile)
        {
            throw new NotImplementedException();
        }
    }
}

using Image2U.Service.Models.Image;
using Image2U.Service.Models.Zip;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Image2U.Service.Interface
{
    public interface IZipService
    {
        byte[] GetZipData(IEnumerable<ZipData> data);
    }
}

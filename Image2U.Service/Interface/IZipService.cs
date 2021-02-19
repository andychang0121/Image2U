using Image2U.Service.Models.Zip;
using System.Collections.Generic;

namespace Image2U.Service.Interface
{
    public interface IZipService
    {
        byte[] GetZipData(IEnumerable<ZipData> data);
    }
}

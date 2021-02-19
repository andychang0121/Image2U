using Image2U.Service.Models.Image;
using Image2U.Service.Models.Zip;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Image2U.Service.Models;

namespace Image2U.Service.Handler
{
    public interface IConvertHandler
    {
        Task<byte[]> ConvertProcessAsync(Stream stream, ProcessData processData);
    }
}

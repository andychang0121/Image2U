using Image2U.Service.Models;
using System.IO;
using System.Threading.Tasks;

namespace Image2U.Service.Handler
{
    public interface IConvertHandler
    {
        Task<byte[]> ConvertProcessAsync(Stream stream, ProcessData processData);
    }
}

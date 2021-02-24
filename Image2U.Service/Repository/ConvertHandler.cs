using Image2U.Service.Handler;
using Image2U.Service.Helper;
using Image2U.Service.Interface;
using Image2U.Service.Models;
using Image2U.Service.Models.Image;
using Image2U.Service.Models.Zip;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Image2U.Service.Repository
{
    public class ConvertHandler : IConvertHandler
    {
        private readonly IImageService _imageService;
        private readonly IZipService _izipService;

        public ConvertHandler()
        {
            _imageService = new ImageService();
            _izipService = new ZipService();
        }

        public async Task<byte[]> ConvertProcessAsync(Stream stream, ProcessData processData)
        {
            IEnumerable<ZipData> zipDatas = await ConvertProcessAsync(
                stream, processData.FileName, processData.IsPortait, processData.OutputDict);

            byte[] bytes = await Task.Run(() => _izipService.GetZipData(zipDatas));

            return bytes;
        }

        private async Task<IEnumerable<ZipData>> ConvertProcessAsync(Stream stream, string fileName, bool isPortait, Dictionary<string, ImageOutput> outputProfile)
        {
            List<ZipData> zipDatas = new List<ZipData>();

            foreach (KeyValuePair<string, ImageOutput> ec in outputProfile)
            {
                string ecName = ec.Key;
                ImageOutput ecSetting = ec.Value;

                byte[] bytes = await _imageService.GetImageBytesAsync(stream, fileName, isPortait, ecSetting.Width,
                    ecSetting.MaxHeight, ecName);

                string zipFileName = ZipHelper.GetZipFileName(fileName, ecSetting.Width, ecSetting.MaxHeight, ecName);

                ZipData zipData = new ZipData(bytes, zipFileName, string.Empty);

                zipDatas.Add(zipData);
            }

            return zipDatas;
        }
    }
}

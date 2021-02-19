using Image2U.Service.Interface;
using Image2U.Service.Models.Image;
using Image2U.Service.Models.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Image2U.Service.Repository
{
    public class ZipService : IZipService
    {
        public byte[] GetZipData(IEnumerable<ZipData> zipDatas)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(ms, ZipArchiveMode.Update))
                {
                    foreach (ZipData zip in zipDatas)
                    {
                        string zipFileName = $"{zip.FileName}";

                        ZipArchiveEntry entry = zipArchive.CreateEntry(zipFileName);

                        using (Stream stream = entry.Open())
                        {
                            byte[] buff = zip.Bytes;
                            stream.Write(buff, 0, buff.Length);
                        }
                    }
                }
                return ms.ToArray();
            }
        }
    }
}

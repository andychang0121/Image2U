using Image2U.Service.Models.Zip;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Image2U.Service.Helper
{
    public class ZipHelper
    {
        public static string GetZipFileName(string fileName, int width, int height, string folderName, bool incldeFolder = true)
        {
            if (string.IsNullOrEmpty(fileName)) return string.Empty;

            string firstSection = fileName.Split('.').FirstOrDefault();
            string extSection = fileName.Split('.').LastOrDefault();

            return !incldeFolder
                ? $"({folderName})-{firstSection}-{width}x{height}.{extSection}"
                : $"{folderName}\\{firstSection}-{width}x{height}.{extSection}";
        }

        public static byte[] ZipData(IEnumerable<ZipData> data)
        {
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                {
                    foreach (ZipData zip in data)
                    {
                        string zipFileName = $"{zip.FileName}";

                        ZipArchiveEntry entry = zipArchive.CreateEntry(zipFileName);

                        using (Stream entryStream = entry.Open())
                        {
                            byte[] buff = zip.Bytes;
                            entryStream.Write(buff, 0, buff.Length);
                        }
                    }
                }
                return zipStream.ToArray();
            }
        }

        public static Dictionary<string, byte[]> UnzipData(byte[] zip)
        {
            var dict = new Dictionary<string, byte[]>();
            using (var msZip = new MemoryStream(zip))
            {
                using (var archive = new ZipArchive(msZip, ZipArchiveMode.Read))
                {
                    archive.Entries.ToList().ForEach(entry =>
                    {
                        //e.FullName可取得完整路徑
                        if (string.IsNullOrEmpty(entry.Name)) return;
                        using (var entryStream = entry.Open())
                        {
                            using (var msEntry = new MemoryStream())
                            {
                                entryStream.CopyTo(msEntry);
                                dict.Add(entry.Name, msEntry.ToArray());
                            }
                        }
                    });
                }
            }
            return dict;
        }
    }
}

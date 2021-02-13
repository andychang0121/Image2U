using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Image2U.Web.Helper;

namespace Image2U.Web.Models
{
    public class RequestData
    {
        public bool IsCustomeSpec => CustomHeight.HasValue && CustomWidth.HasValue;

        public int? CustomWidth { get; set; }

        public int? CustomHeight { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public string IsPortait { set; get; }

        public int Size { get; set; }

        public string FileName { get; set; }

        public string Base64 { get; set; }

        public Stream Stream { get; set; }

        public string Type { get; set; }
    }

    public static class RequestDataExtension
    {
        public static Stream GetStream(this string base64String)
        {
            byte[] bytes = base64String.GetBytesFromBase64();

            Stream stream = new MemoryStream(bytes);

            stream.Position = 0;

            return stream;
        }
    }
}
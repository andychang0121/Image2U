using System;

namespace Image2U.Web.Models
{
    public static class RequestDataExtension
    {
        public static bool IsPortsait(this RequestData requestData)
            => requestData.Height > requestData.Width;

        public static bool ValidRequestData(this RequestData requestData)
            => !string.IsNullOrEmpty(requestData.Base64)
               && !string.IsNullOrEmpty(requestData.FileName);
    }

    public struct RequestData
    {
        public string Base64 { get; set; }

        public string FileName { get; set; }

        public int Size { get; set; }

        public string Type { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsCustomSize => CustomWidth.HasValue && CustomHeight.HasValue;

        public int? CustomWidth { get; set; }

        public int? CustomHeight { get; set; }

    }
}
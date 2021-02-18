namespace Image2U.Service.Models
{
    public struct RequestData
    {
        public string Base64 { get; set; }

        public string FileName { get; set; }

        public int Size { get; set; }

        public string Type { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsCustomSize => CustomWidth.HasValue && CustomHeight.HasValue;

        public bool IsPortait => Height > Width;

        public int? CustomWidth { get; set; }

        public int? CustomHeight { get; set; }

    }
}
namespace Image2U.Service.Models.Image
{
    public class OutputSize
    {
        public int Width { get; set; }

        public int MaxHeight { get; set; }

        public int MinHeight { get; set; }
    }

    public class ImageOutput : OutputSize
    {
        public string EcName { get; set; }

        public string Category { get; set; }

        public int Resolution { get; set; }

        public int DPI { get; set; }
    }
}

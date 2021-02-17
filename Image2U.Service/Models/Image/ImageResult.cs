using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Image2U.Service.Models.Image
{
    public struct ImageResult
    {
        public string FileName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] Bytes { get; set; }
    }
}
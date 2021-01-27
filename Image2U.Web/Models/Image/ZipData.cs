using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Image2U.Web.Models.Image
{
    public class ZipData
    {
        public string FileName { get; set; }

        public byte[] Bytes { get; set; }

        public string FolderName { get; set; }
    }
}
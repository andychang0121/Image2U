using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Image2U.Web.Models
{
    public struct RequestData
    {
        public string Base64 { get; set; }

        public string FileName { get; set; }

        public int Size { get; set; }

        public string Type { get; set; }
    }
}
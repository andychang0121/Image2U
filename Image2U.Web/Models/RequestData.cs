using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace Image2U.Web.Models
{
    public class RequestFormData
    {
        [JsonProperty("isPortaits")]
        public string IsPortaits { set; get; }

        public IEnumerable<HttpPostedFile> File { set; get; }
    }
}
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace Image2U.Web.Models
{
    public class RequestData
    {
        [JsonProperty("isPortaits")]
        public string IsPortaits { set; get; }

        public IEnumerable<HttpPostedFileBase> File { set; get; }
    }
}
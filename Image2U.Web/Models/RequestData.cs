using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Image2U.Web.Models
{
    public class RequestFormData
    {
        private readonly string _customWidth;

        private readonly string _customHeight;

        public RequestFormData(string customeWidth, string customHeight)
        {
            _customWidth = customeWidth;
            _customHeight = customHeight;
        }

        private static bool IsDigital(string numberStr)
        {
            bool isDigital = !string.IsNullOrWhiteSpace(numberStr) && numberStr.All(char.IsDigit);

            if (!isDigital) return false;

            int number = int.Parse(numberStr);

            return number > 0;
        }

        public bool IsCustomeSpec =>
            IsDigital(_customWidth) && IsDigital(_customHeight);

        public int CustomWidth => IsCustomeSpec ? int.Parse(_customWidth) : 0;

        public int CustomHeight => IsCustomeSpec ? int.Parse(_customHeight) : 0;

        [JsonProperty("isPortaits")]
        public string IsPortaits { set; get; }

        public Dictionary<string, string> FormData { get; set; }

        public IEnumerable<object> File { set; get; }
    }
}
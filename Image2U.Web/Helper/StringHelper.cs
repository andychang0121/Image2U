using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Image2U.Web.Helper
{
    public static class StringHelper
    {
        public static bool[] GetStringToArray(string stringValue)
            => stringValue
                .GetStringArray()
                .GetStingArrayToBoolean()
                ?.ToArray();

        public static string[] GetStringArray(this string value)
        => !string.IsNullOrEmpty(value) ? value.Split(',') : null;

        public static IEnumerable<bool> GetStingArrayToBoolean(this string[] values)
        => values?.Select(v => v.ToLower() == "true").ToList();

        public static byte[] GetBytesFromBase64(this string base64String)
            => base64String.GetBase64String();

        private static byte[] GetBase64String(this string base64String)
        {
            base64String = base64String.Split(',').LastOrDefault();

            if (string.IsNullOrEmpty(base64String)
                || base64String.Length % 4 != 0
                || base64String.Contains(" ")
                || base64String.Contains("\t")
                || base64String.Contains("\r")
                || base64String.Contains("\n"))
                return null;
            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch (Exception)
            {
                // Handle the exception
            }
            return null;
        }
    }
}

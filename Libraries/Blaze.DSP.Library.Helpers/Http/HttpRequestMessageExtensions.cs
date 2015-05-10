namespace Blaze.DSP.Library.Helpers.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    // TODO: Clean this up, the values are being parsed multiple times, should be parsed once then returned based on look up
    public static class HttpRequestMessageExtensions
    {
        public static Dictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            // NOTE: This will throw if there are multiple keys with the same name
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
            {
                return null;
            }

            var match = queryStrings.FirstOrDefault(kv => String.Compare(kv.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
            return string.IsNullOrEmpty(match.Value) ? null : match.Value;
        }

        public static async Task<string> GetFormString(this HttpRequestMessage request, string key)
        {
            var formStrings = await request.Content.ReadAsFormDataAsync();
            return formStrings == null ? null : formStrings[key];
        }

        public static string GetHeader(this HttpRequestMessage request, string key)
        {
            IEnumerable<string> keys;
            return !request.Headers.TryGetValues(key, out keys) ? null : keys.First();
        }

        public static string GetCookie(this HttpRequestMessage request, string cookieName)
        {
            var cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();
            return cookie != null ? cookie[cookieName].Value : null;
        }
    }
}
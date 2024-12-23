using Newtonsoft.Json;
using System.Web;

namespace Knab.Cryptocurrency.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public static string ToUrl(this Dictionary<string, string> queryParams, string baseUrl, string apiUrl)
        {
            var builder = new UriBuilder(new Uri(new Uri(baseUrl), apiUrl));
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var param in queryParams)
            {
                query[param.Key] = param.Value;
            }

            builder.Query = query.ToString();
            return builder.ToString();
        }

        public static async Task<T> ParseJsonResponseAsync<T>(this HttpResponseMessage response, string endpointName, CancellationToken cancellationToken)
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            var result = JsonConvert.DeserializeObject<T>(responseBody);
            return result ?? throw new Exception($"Failed to deserialize the {endpointName} response.");
        }
    }
}

using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;


namespace Simbrella.LoanManagement.TestHelpers.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadBodyAsAsync<T>(this HttpContent content)
        {
            var body = await content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(body);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Simbrella.Infrastructure.TestHelpers.Utils
{
    public static class ConversionUtils
    {
        public static async IAsyncEnumerable<T> ConvertToAsyncEnumerable<T>(T item)
        {
            yield return item;

            await Task.CompletedTask;
        }
    }
}
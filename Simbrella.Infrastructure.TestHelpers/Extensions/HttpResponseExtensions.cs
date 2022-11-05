using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Xunit;


namespace Simbrella.LoanManagement.TestHelpers.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task AssertBadRequestAsync(this HttpResponseMessage response, string validationKey, string validationMessage)
        {
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var details = await response.Content.ReadBodyAsAsync<ValidationProblemDetails>().ConfigureAwait(false);

            Assert.NotNull(details);

            Assert.NotNull(details.Errors);

            Assert.Contains(validationKey, details.Errors.Keys);

            Assert.Contains(validationMessage, details.Errors[validationKey]);
        }
    }
}
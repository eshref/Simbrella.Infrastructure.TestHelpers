using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Moq;
using Moq.Language.Flow;
using Moq.Protected;

using Newtonsoft.Json;


namespace Simbrella.Infrastructure.TestHelpers.Extensions
{
    public static class MockHttpHandlerExtensions
    {
        public static void SetupRemoteCall(
            this Mock<HttpMessageHandler> messageHandlerMock,
            string uri,
            object remoteResponse)
        {
            messageHandlerMock.SetupRemoteCall(uri, remoteResponse, HttpStatusCode.OK);
        }

        public static void SetupRemoteCall(
            this Mock<HttpMessageHandler> messageHandlerMock,
            string uri,
            object remoteResponse,
            HttpStatusCode httpStatusCode)
        {
            messageHandlerMock.setupSend(uri)
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = httpStatusCode,
                    Content = new StringContent(JsonConvert.SerializeObject(remoteResponse), Encoding.UTF8, "application/json")
                }).Verifiable();
        }

        public static void SetupRemoteCall(
            this Mock<HttpMessageHandler> messageHandlerMock,
            string uri,
            object remoteResponse,
            Action<HttpRequestMessage, CancellationToken> callback)
        {
            messageHandlerMock.setupSend(uri)
                .Callback(callback)
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(remoteResponse), Encoding.UTF8, "application/json")
                }).Verifiable();
        }

        public static void SetupRemoteCall(
            this Mock<HttpMessageHandler> messageHandlerMock,
            string uri,
            HttpContent remoteContent)
        {
            messageHandlerMock.SetupRemoteCall(uri, remoteContent, HttpStatusCode.OK);
        }

        public static void SetupRemoteCall(
            this Mock<HttpMessageHandler> messageHandlerMock,
            string uri,
            HttpContent remoteContent,
            HttpStatusCode httpStatusCode)
        {
            messageHandlerMock.setupSend(uri)
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = httpStatusCode,
                    Content = remoteContent
                }).Verifiable();
        }

        public static void SetupRemoteCall(
            this Mock<HttpMessageHandler> messageHandlerMock,
            string uri,
            Exception ex)
        {
            messageHandlerMock.setupSend(uri).ThrowsAsync(ex).Verifiable();
        }
        

        private static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setupSend(
            this Mock<HttpMessageHandler> messageHandlerMock,
            string uri)
        {
            return messageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    MockHttpHandlerExtensions.compareUris(request, uri)),
                ItExpr.IsAny<CancellationToken>());
        }

        private static bool compareUris(HttpRequestMessage actualMessage, string expectedUri)
        {
            var actualUri = actualMessage.RequestUri.ToString();

            return actualUri.Equals(expectedUri, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
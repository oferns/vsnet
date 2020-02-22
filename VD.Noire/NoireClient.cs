namespace VD.Noire {

    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public class NoireClient : INoireClient {
        private readonly IHttpClientFactory factory;
        private readonly NoireOptions options;
        private readonly JsonSerializerOptions jsonOptions;

        public NoireClient(IHttpClientFactory factory, NoireOptions options) {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new DateTimeOffsetConverter());
        }

        public async Task<NoireResponse> CreateCheckout(Checkout request, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"/v1/checkouts");
            var httprequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}&{request}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;

            using var client = this.factory.CreateClient($"Noire-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);
                 
            var obj = await HandleResponse(response);
            return obj;
        }

        public async Task<NoireResponse> CheckoutStatus(string resourcePath, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"{resourcePath}?entityId={options.EntityId}");
            var httprequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            using var client = this.factory.CreateClient($"Noire-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            return await HandleResponse(response);
        }

        private T DeserializeStream<T>(Stream stream) {
            using var jsonStreamReader = new Utf8JsonStreamreader(stream, 32 * 1024);
            jsonStreamReader.Read();     // TODO: Why do I have to do this?
            return jsonStreamReader.Deserialise<T>(this.jsonOptions);
        }

        private async Task<NoireResponse> HandleResponse(HttpResponseMessage responseMessage) {

            switch (responseMessage.StatusCode) {
                case HttpStatusCode.OK:
                case HttpStatusCode.BadRequest: {
                        using var responseStream = await responseMessage.Content.ReadAsStreamAsync();
                        return DeserializeStream<NoireResponse>(responseStream);
                    }
                case HttpStatusCode.Unauthorized:
                    return new NoireResponse { Result = new NoireResult { Description = $"Endpoint responded with 401: Unauthorised. This means the bearer token is either wrong or missing." } };
                case HttpStatusCode.Forbidden:
                    return new NoireResponse { Result = new NoireResult { Description = $"Endpoint responded with 403: Forbidden. This means an invalid access token was provided." } };
                case HttpStatusCode.NotFound:
                    return new NoireResponse { Result = new NoireResult { Description = $"Endpoint responded with 404: Not Found." } };
                default:            // TODO: Add more info here
                    return new NoireResponse { Result = new NoireResult { Description = $"Noire responded with an unrecognised response." } };
            }
        }

    }
}

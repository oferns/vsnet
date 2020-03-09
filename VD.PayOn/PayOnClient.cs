namespace VD.PayOn {

    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Serilog;
    using VD.PayOn.BackOffice;

    public class PayOnClient : IPayOnClient {

        private readonly IHttpClientFactory factory;
        private readonly PayOnOptions options;
        private readonly ILogger log;
        private readonly JsonSerializerOptions jsonOptions;

        public Uri BaseUri => options.BaseUri;

        public PayOnClient(IHttpClientFactory factory, PayOnOptions options,  ILogger log, JsonSerializerOptions jsonOptions = default) {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            this.jsonOptions = jsonOptions ?? new JsonSerializerOptions();

            foreach (var converter in jsonOptions.Converters) {
                if (converter.CanConvert(typeof(DateTimeOffset))) return;
            }
            jsonOptions.Converters.Add(new DateTimeOffsetConverter());
        }

        public async Task<PayOnResponse> CreateCheckout(Checkout checkout, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"/v1/checkouts");
            
            var httprequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}&{checkout}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;

            log.Information($"PayOn-{options.EntityId} sending CreateCheckout request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");
            
            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} CreateCheckout response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");


            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();

                log.Information($"PayOn-{options.EntityId} received CreateCheckout response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");
                var result = JsonSerializer.Deserialize<PayOnResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        }

        public async Task<PayOnResponse> CheckoutStatus(string resourceId, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"/v1/checkouts/{resourceId}/payment?entityId={options.EntityId}");
            var httprequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            log.Information($"PayOn-{options.EntityId} sending CheckoutStatus request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} CheckoutStatus response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();

                log.Information($"PayOn-{options.EntityId} received CheckoutStatus response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");
                var result = JsonSerializer.Deserialize<PayOnResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        }

        public async Task<PayOnResponse> Capture(Capture capture, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"/v1/PayOn");
            var httprequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}&{capture}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;

            log.Information($"PayOn-{options.EntityId} sending Capture request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} Capture response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();

                log.Information($"PayOn-{options.EntityId} received Capture response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");
                var result = JsonSerializer.Deserialize<PayOnResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        }

        public async Task<PayOnResponse> Refund(Refund refund, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"/v1/PayOn");
            var httprequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}&{refund}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;

            log.Information($"PayOn-{options.EntityId} sending Refund request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} Refund response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();

                log.Information($"PayOn-{options.EntityId} received Capture response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");
                var result = JsonSerializer.Deserialize<PayOnResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        }

        public async Task<PayOnResponse> Credit(Credit credit, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"/v1/PayOn");
            
            using var httprequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}&{credit}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;


            log.Information($"PayOn-{options.EntityId} sending Refund request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}. {credit}");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} Refund response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();

                log.Information($"PayOn-{options.EntityId} received Refund response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");

                var result = JsonSerializer.Deserialize<PayOnResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        }

        public async Task<PayOnResponse> Rebill(Rebill rebill, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"/v1/PayOn");
            var httprequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}&{rebill}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;

            log.Information($"PayOn-{options.EntityId} sending Rebill request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}. {rebill}");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);


            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} Rebill response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();
                log.Information($"PayOn-{options.EntityId} received Rebill response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");

                var result = JsonSerializer.Deserialize<PayOnResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        }

        public async Task<PayOnResponse> RegisterCard(RegisterCard registerCard, CancellationToken cancel) {
            var requestUri = new Uri(options.BaseUri, $"/v1/checkouts");
            var httprequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}&{registerCard}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;

            log.Information($"PayOn-{options.EntityId} sending RegisterCard request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}. {registerCard}");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} RegisterCard response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();
                log.Information($"PayOn-{options.EntityId} received RegisterCard response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");
                var result = JsonSerializer.Deserialize<PayOnResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        }

        public async Task<RegisterCardResponse> RegisterCardStatus(string registrationId, CancellationToken cancel) {
            var requestUri = new Uri(options.BaseUri, $"/v1/checkouts/{registrationId}/registration?entityId={options.EntityId}");
            var httprequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            log.Information($"PayOn-{options.EntityId} sending RegisterCardStatus request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} RegisterCardStatus response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();
                log.Information($"PayOn-{options.EntityId} received RegisterCard response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");
                var result = JsonSerializer.Deserialize<RegisterCardResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new RegisterCardResponse { 
                    Result = new PayOnResult { 
                        Code = $"Http.{response.StatusCode}",  
                        Description = $"Endpoint responded with {response.StatusCode}." 
                    } 
            };
        }

        public async Task<PayOnResponse> DeRegisterCard(DeRegisterCard deregisterCard, CancellationToken cancel) {
            var requestUri = new Uri(options.BaseUri, $"/v1/registrations/{deregisterCard}?entityId={options.EntityId}");
            var httprequest = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;

            log.Information($"PayOn-{options.EntityId} sending DeRegisterCard request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}. {deregisterCard}");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);

            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} DeRegisterCard response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();
                log.Information($"PayOn-{options.EntityId} received DeRegisterCard response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");
                var result = JsonSerializer.Deserialize<RegisterCardResponse>(responseString, this.jsonOptions);
                result.OriginalResponse = responseString;
                return result;
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        }

        public async Task<PayOnResponse> Reversal(Reversal reversal, CancellationToken cancel) {

            var requestUri = new Uri(options.BaseUri, $"/v1/PayOn");
            var httprequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            httprequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            var content = new ByteArrayContent(Encoding.ASCII.GetBytes($"entityId={options.EntityId}&{reversal}"));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httprequest.Content = content;

            log.Information($"PayOn-{options.EntityId} sending Reversal request to {requestUri.AbsoluteUri} at {DateTimeOffset.Now}. {reversal}");

            using var client = this.factory.CreateClient($"PayOn-{options.EntityId}");
            using var response = await client.SendAsync(httprequest, HttpCompletionOption.ResponseHeadersRead, cancel)
                .ConfigureAwait(false);


            log.Information($"PayOn-{options.EntityId} received HTTP {response.StatusCode} Reversal response from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.");

            if (response.IsSuccessStatusCode || response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                var responseString = await response.Content.ReadAsStringAsync();
                log.Information($"PayOn-{options.EntityId} received Reversal response JSON from {requestUri.AbsoluteUri} at {DateTimeOffset.Now}.{responseString}");
                return JsonSerializer.Deserialize<PayOnResponse>(responseString, this.jsonOptions);
            }

            return new PayOnResponse {
                Result = new PayOnResult {
                    Code = $"Http.{response.StatusCode}",
                    Description = $"Endpoint responded with {response.StatusCode}."
                }
            };
        } 
    }
}

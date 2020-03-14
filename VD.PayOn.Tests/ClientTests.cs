
namespace VD.PayOn.Tests {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Protected;

    [TestClass]
    public class CheckoutRequestTests {

        [TestClass]
        public class ToStringTests {


            [TestMethod]
            public void ShouldReturnMinimumInfo() {
                // Arrange & Act
                var checkoutRequest = new Checkout(100, "GBP", PaymentType.Debit);

                // Assert
                Assert.AreEqual("amount=100&currency=GBP&paymentType=DB", checkoutRequest.ToString());


            }

            [TestMethod]
            public void ShouldReturnFullInfo() {

                // Arrange & Act
                var checkoutRequest = new Checkout(
                        100,
                        "GBP",
                        PaymentType.Debit,
                        Array.Empty<string>(),
                        "VISA",
                        new Dictionary<string, PaymentType> { { "BOLETO", PaymentType.PreAuthorization } },
                        10,
                        "TEST",
                        "TEST",
                        "TEST",
                        "TEST",
                        true,
                        TransactionCategory.eCommerce);

                // Assert
                Assert.AreEqual(@"amount=100&currency=GBP&paymentType=DB&paymentBrand=VISA&overridePaymentType[BOLETO]=PA&taxAmount=10&descriptor=TEST&merchantTransactionId=TEST&merchantInvoiceId=TEST&merchantMemo=TEST&transactionCategory=EC", checkoutRequest.ToString());

            }
        }
    }


    [TestClass]
    public class ClientTests {

        internal static PayOnOptions options = new PayOnOptions {
            //Token = "0000000000000000000000000000000000000000000000000000000000==",
            //EntityId = "8a8a8a8a8a8a8a8a8a8a8a8a8a8a8a8a",
            //BaseUri = new Uri("test://test")

            Token = "OGE4Mjk0MTc1YjE0NDFiNzAxNWIxOTE3YTA4MzE1ZDV8bmJtUUdrOXE3ag==",
            EntityId = "8a8294175b1441b7015b1917a03415d1",
            BaseUri = new Uri("https://test.oppwa.com/")
        };


        private Mock<HttpMessageHandler> GetHandler(string response) {

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage() {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(response),
               })
               .Verifiable();
            return handlerMock;
        }


        private Mock<ILogger<PayOnClient>> GetLooseLog() {
            return new Mock<ILogger<PayOnClient>>(MockBehavior.Loose);        
        }

        [TestMethod]
        public async Task ShouldCreateSimpleCheckout() {

            var response = File.ReadAllText("Responses/create_checkout.json");

            var handlerMock = GetHandler(response);
            var mockFactory = new Moq.Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() =>
                // new HttpClient(handlerMock.Object, false)
                new HttpClient()
            );


            var client = new PayOnClient(mockFactory.Object, options, GetLooseLog().Object);

            var checkoutRequest = new Checkout(100, "GBP", PaymentType.Debit);

            var result = await client.CreateCheckout(checkoutRequest, CancellationToken.None);


        }

        [TestMethod]
        public async Task ShouldCreateCompleteCheckout() {

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(() => new HttpClient());

            var client = new PayOnClient(mockFactory.Object, options, GetLooseLog().Object);

            var checkoutRequest = new Checkout(
                100,
                "GBP",
                PaymentType.Debit,
                Array.Empty<string>(),
                "VISA",
                new Dictionary<string, PaymentType> { { "BOLETO", PaymentType.PreAuthorization } },
                10,
                "TEST",
                "TEST",
                "TEST",
                "TEST",
                true,
                TransactionCategory.eCommerce);

            var result = await client.CreateCheckout(checkoutRequest, CancellationToken.None);

        }
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace VD.Noire {
    public interface INoireClient {
        Task<NoireResponse> CheckoutStatus(string resourcePath, CancellationToken cancel);
        Task<NoireResponse> CreateCheckout(Checkout request, CancellationToken cancel);
    }
}
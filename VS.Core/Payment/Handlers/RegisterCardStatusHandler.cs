
namespace VS.Core.Payment.Handlers {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class RegisterCardStatusHandler : IRequestHandler<RegisterCardStatusRequest, RegisterCardStatusResponse> {


        public Task<RegisterCardStatusResponse> Handle(RegisterCardStatusRequest request, CancellationToken cancellationToken) {
            return Task.FromResult(new RegisterCardStatusResponse(false, default, "NO PROVIDER", default, DateTimeOffset.UtcNow));
        }
    }
}

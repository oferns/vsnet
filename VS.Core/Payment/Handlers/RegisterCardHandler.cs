namespace VS.Core.Payment.Handlers {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using VS.Abstractions.Logging;
    using VS.Core.Payment;

    public class RegisterCardHandler : IRequestHandler<RegisterCardRequest, ReqisterCardResponse> {

        public Task<ReqisterCardResponse> Handle(RegisterCardRequest request, CancellationToken cancellationToken) {
            return Task.FromResult(new ReqisterCardResponse(false, "NO PROVIDER AVAILABLE", DateTimeOffset.UtcNow));
        }
    }
}

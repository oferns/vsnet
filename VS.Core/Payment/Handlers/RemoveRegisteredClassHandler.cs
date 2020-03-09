namespace VS.Core.Payment.Handlers {

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class RemoveRegisteredClassHandler : IRequestHandler<RemoveRegisteredCardRequest, bool> {
        
        
        public Task<bool> Handle(RemoveRegisteredCardRequest request, CancellationToken cancellationToken) {
            return Task.FromResult<bool>(false);
        }
    }
}

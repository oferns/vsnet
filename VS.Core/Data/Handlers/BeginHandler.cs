namespace VS.Core.Data.Handlers {

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class BeginHandler : IRequestHandler<Begin> {
        public Task<Unit> Handle(Begin request, CancellationToken cancellationToken) {
            return Task.FromResult(new Unit());
        }
    }
}

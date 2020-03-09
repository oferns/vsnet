namespace VS.Core.Data.Handlers {

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class CommitHandler : IRequestHandler<Commit> {
        public Task<Unit> Handle(Commit request, CancellationToken cancellationToken) {
            return Task.FromResult(new Unit());
        }
    }
}

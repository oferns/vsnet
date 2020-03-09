namespace VS.Core.Data.Handlers {

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class RollbackHandler : IRequestHandler<Rollback> {
        public Task<Unit> Handle(Rollback request, CancellationToken cancellationToken) {
            return Task.FromResult(new Unit());
        }
    }
}

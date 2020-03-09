
namespace VS.Core.Data.Handlers {

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class DeleteHandler<T> : IRequestHandler<Delete<T>, IEnumerable<T>> where T : class {
        public Task<IEnumerable<T>> Handle(Delete<T> request, CancellationToken cancellationToken) {

            return Task.FromResult<IEnumerable<T>>(Array.Empty<T>());

        }
    }
}

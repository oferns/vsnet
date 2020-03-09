
namespace VS.Core.Data.Handlers {

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class CreateHandler<T> : IRequestHandler<Create<T>, IEnumerable<T>> where T : class {
        public Task<IEnumerable<T>> Handle(Create<T> request, CancellationToken cancellationToken) {

            return Task.FromResult<IEnumerable<T>>(Array.Empty<T>());

        }
    }
}

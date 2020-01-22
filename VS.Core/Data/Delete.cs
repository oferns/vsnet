namespace VS.Core.Data {

    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions.Data.Filtering;

    public class Delete<T> : IRequest<IEnumerable<T>> where T : class {

        public IFilter<T> Filter { get; private set; }
    }
}

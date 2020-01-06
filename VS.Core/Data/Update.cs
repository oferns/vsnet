namespace VS.Core.Data {

    using MediatR;
    using System.Collections.Generic;
    using VS.Core.Data.Abstractions;

    public class Update<T> : IRequest<IEnumerable<T>> where T : class {

        public IEnumerable<KeyValuePair<string,object>> PropertiesToUpdate { get; set; }

        public IFilter<T> Filter { get; set; }

    }
}

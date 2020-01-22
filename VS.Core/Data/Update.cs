namespace VS.Core.Data {

    using MediatR;
    using System.Collections.Generic;
    using VS.Abstractions.Data.Filtering;

    public class Update<T> : IRequest<IEnumerable<T>> where T : class {

        public IEnumerable<KeyValuePair<string,object>> PropertiesToUpdate { get; set; }

        public IFilter<T> Filter { get; set; }

    }
}

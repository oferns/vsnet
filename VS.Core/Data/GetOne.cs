namespace VS.Core.Data {

    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Core.Data.Abstractions;


    public class GetOne<T> : IRequest<T> where T : class {

        public IFilter<T> Filter { get; private set; }

    }
}

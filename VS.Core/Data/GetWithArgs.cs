namespace VS.Core.Data {

    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Core.Data.Abstractions;


    /// <summary>
    /// A request to get data from a table returning SQL function
    /// </summary>
    /// <typeparam name="T">The Function type</typeparam>
    /// <typeparam name="A"></typeparam>
    public class GetWithArgs<T, A> : IRequest<IEnumerable<T>> where T :class where A :class {


        public IFilter<T> Filter { get; private set; }

    }
}

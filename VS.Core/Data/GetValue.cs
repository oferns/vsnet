
namespace VS.Core.Data {
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GetValue<A, R> : IRequest<R> where A : class where R : struct {
        public A Arguments { get; set; }
    }
}

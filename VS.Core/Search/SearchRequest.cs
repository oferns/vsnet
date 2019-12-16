
namespace VS.Core.Search {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MediatR;
    using Microsoft.Extensions.Primitives;

    public class SearchRequest : IRequest<Uri[]> {

        public StringValues[] Query { get; set; }
    }
}

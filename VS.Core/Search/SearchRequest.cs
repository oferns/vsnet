
namespace VS.Core.Search {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MediatR;
    using Microsoft.Extensions.Primitives;

    public class SearchRequest : IRequest<SearchResponse[]> {

        public SearchRequest(StringValues[] query) {
            Query = query;
        }

        public StringValues[] Query { get; private set; }
    }
}

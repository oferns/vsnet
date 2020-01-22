namespace VS.Abstractions.Messaging {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PagedBrokerIndex<T> : List<T> {

        public PagedBrokerIndex(IEnumerable<T> source, int pageSize, string curerntToken = null, string nextToken = null, string previousToken = null) : base(source) {
            PageSize = pageSize;
            CurrentContinuationToken = curerntToken;
            NextContinuationToken = nextToken;
            PreviousContinuationToken = previousToken;
        }

        public int PageSize { get; private set; }

        public string NextContinuationToken { get; private set; }

        public string CurrentContinuationToken { get; private set; }

        public string PreviousContinuationToken { get; private set; }

    }
}

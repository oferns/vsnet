namespace VS.Abstractions.Storage.Paging {

    using System.Collections;
    using System.Collections.Generic;

    public class PagedIndex : List<IndexItem> {

        public PagedIndex(IEnumerable<IndexItem> source, int pageSize, string curerntToken = null, string nextToken = null, string previousToken = null) : base(source) {
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
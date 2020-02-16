namespace VS.Core.Search {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SearchResponse  {

        public int Position { get; private set; }
        public decimal Rank { get; private set; }

        public Uri Path { get; private set; }

    }
}

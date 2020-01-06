namespace VS.Core.Data {

    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Core.Data.Abstractions;
    using VS.Core.Data.Paging;

    public class GetList<T> : IRequest<PagedList<T>> where T : class {

        public int StartFrom { get; set; }
        public int PageSize { get; set; }
        public IFilter<T> Filter { get; set; }
        public ISorter<T> Sorter { get; set; }
      
    }
}
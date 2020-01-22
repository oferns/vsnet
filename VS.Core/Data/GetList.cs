namespace VS.Core.Data {

    using MediatR;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Data.Paging;
    using VS.Abstractions.Data.Sorting;


    public class GetList<T> : IRequest<PagedList<T>> where T : class {

        public GetList(int startFrom, int pageSize, IFilter<T> filter, ISorter<T> sorter) {
            StartFrom = startFrom;
            PageSize = pageSize;
            Filter = filter;
            Sorter = sorter;
        }

        public int StartFrom { get; private set; }
        public int PageSize { get; private set; }
        public IFilter<T> Filter { get; private set; }
        public ISorter<T> Sorter { get; private set; }

    }


    public class GetList<F, T> : IRequest<PagedList<T>> where F : class where T : class {

        public GetList(F function, int startFrom, int pageSize, IFilter<T> filter, ISorter<T> sorter) {
            Function = function;
            StartFrom = startFrom;
            PageSize = pageSize;
            Filter = filter;
            Sorter = sorter;
        }

        public F Function { get; private set; }
        public int StartFrom { get; private set; }
        public int PageSize { get; private set; }
        public IFilter<T> Filter { get; private set; }
        public ISorter<T> Sorter { get; private set; }

    }


}


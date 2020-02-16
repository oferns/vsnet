namespace VS.Core.Data {

    using MediatR;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Data.Paging;
    using VS.Abstractions.Data.Sorting;

    public class GetList<T> : IRequest<PagedList<T>> where T : class {

        public GetList(IFilter<T> filter = default, ISorter<T> sorter = default, IPager pager = default) {
            Filter = filter;
            Sorter = sorter;
            Pager = pager;
        }
              
        public IFilter<T> Filter { get; private set; }
        public ISorter<T> Sorter { get; private set; }
        public IPager Pager { get; private set; }
    }

    public class GetList<F, T> : IRequest<PagedList<T>> where F : class where T : class {

        public GetList(F function, IFilter<T> filter = default, ISorter<T> sorter = default, IPager pager = default) {
            Function = function;
            Filter = filter;
            Sorter = sorter;
            Pager = pager;
        }

        public F Function { get; private set; }
        public IFilter<T> Filter { get; private set; }
        public ISorter<T> Sorter { get; private set; }
        public IPager Pager { get; private set; }
    }
}
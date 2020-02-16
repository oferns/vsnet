namespace VS.Core.Data {

    using MediatR;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Data.Sorting;

    public class GetOne<T> : IRequest<T> where T : class {

        public GetOne(IFilter<T> filter = default, ISorter<T> sorter = default) {
            Filter = filter;
            Sorter = sorter;
        }

        public IFilter<T> Filter { get; private set; }

        public ISorter<T> Sorter { get; private set; }
    }


    public class GetOne<F, T> : IRequest<T> where F : class where T : class {

        public GetOne(F function, IFilter<T> filter = default, ISorter<T> sorter = default) {
            Function = function;
            Filter = filter;
            Sorter = sorter;
        }

        public F Function { get; private set; }

        public IFilter<T> Filter { get; private set; }

        public ISorter<T> Sorter { get; private set; }
    }
}

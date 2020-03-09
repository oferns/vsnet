namespace VS.Abstractions.Data.Filtering {
    
    public interface IFilterService<T> where T : class {

        IFilter<T> Filter { get; }

        string GetQuery(IFilter<T> filter);

    }
}
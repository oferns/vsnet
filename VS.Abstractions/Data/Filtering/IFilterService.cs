namespace VS.Abstractions.Data.Filtering {
    
    public interface IFilterService<T> where T : class {

        IFilter<T> GetFilter();

        string GetQuery(IFilter<T> filter);

    }
}
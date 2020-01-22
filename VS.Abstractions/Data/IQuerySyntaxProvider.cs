namespace VS.Abstractions.Data {
    using System.Collections.Generic;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Data.Paging;
    using VS.Abstractions.Data.Sorting;

    public interface IQuerySyntaxProvider<T> where T : class {

        string Count(IFilter<T> filter, out IDictionary<string, object> parameters);

        string Select(IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters);

        string Select(IEnumerable<string> columns, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters);

        string Select<A>(A args, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) where A : class;

        string Select<A>(A args, IEnumerable<string> columns, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) where A : class;

        string Insert(IEnumerable<T> entities, out IDictionary<string, object> parameters);

        string Update(IDictionary<string, object> properties, IFilter<T> filter, out IDictionary<string, object> parameters);

        string Delete(IFilter<T> filter, out IDictionary<string, object> parameters);
    }
}

namespace VS.Abstractions.Data.Filtering {

    using System.Collections.Generic;

    public interface IFilter<T> : ICollection<ClauseEntry<T>>, IClause<T> where T : class {

        void Add(IClause<T> clause, FilterOp op);
    }
}

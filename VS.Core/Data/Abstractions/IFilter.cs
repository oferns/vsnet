namespace VS.Core.Data.Abstractions {

    using System.Collections.Generic;
    using VS.Core.Data.Filtering;

    public interface IFilter<T> : ICollection<ClauseEntry<T>>, IClause<T> where T : class {

        void Add(IClause<T> clause, FilterOp op);
    }
}

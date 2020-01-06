namespace VS.Core.Data.Filtering {

    using VS.Core.Data.Abstractions;

    public class ClauseEntry<T> where T : class {

        public ClauseEntry(FilterOp op, IClause<T> clause) {
            Op = op;
            Clause = clause;
        }

        public FilterOp Op { get; private set; }
        public IClause<T> Clause { get; private set; }
    }
}

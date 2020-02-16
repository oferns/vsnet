namespace VS.Mvc._Services {
    using System;
    using VS.Abstractions;
    using VS.Abstractions.Data;
    using VS.Abstractions.Data.Sorting;
    using VS.Abstractions.Logging;

    public class QueryStringSorterService<T> : ISorterService<T> where T : class {


        private readonly IContext context;
        private readonly IMetaData<T> metaData;
        private readonly ILog log;
        private ISorter<T> sorter;

        public QueryStringSorterService(IContext context, IMetaData<T> metaData, ILog log) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.metaData = metaData ?? throw new ArgumentNullException(nameof(metaData));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }


        public ISorter<T> GetSorter() {
            if (sorter is object) {
                return sorter;
            }


            sorter = new Sorter<T>();

            return sorter;
        }
    }
}

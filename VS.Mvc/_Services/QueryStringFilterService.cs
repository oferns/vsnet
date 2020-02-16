namespace VS.Mvc._Services {
    using System;
    using System.ComponentModel;
    using System.Text;
    using VS.Abstractions;
    using VS.Abstractions.Data;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Logging;

    public class QueryStringFilterService<T> : IFilterService<T> where T : class {

        private readonly IContext context;
        private readonly IMetaData<T> metaData;
        private readonly ILog log;
        private IFilter<T> filter;
        
        public QueryStringFilterService(IContext context, IMetaData<T> metaData, ILog log) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.metaData = metaData ?? throw new ArgumentNullException(nameof(metaData));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public IFilter<T> GetFilter() {
            if (filter is object) {
                return filter;
            }

            filter = new Filter<T>();

            foreach (var item in context.Query) {

                DbFieldInfo fieldInfo = default;

                foreach (var field in metaData.Fields) {

                    if (field.Property.Name.Equals(item.Key, StringComparison.OrdinalIgnoreCase) // Match the Property name
                        || field.NativeName.Equals(item.Key, StringComparison.OrdinalIgnoreCase) // Match the Field name
                    ) {
                        fieldInfo = field;

                        break;
                    }
                }

                if (fieldInfo.Equals(default)) {
                    continue; // We didnt find a property that matches the query
                }

                // Get the property type
                var isNullable = true;
                var proptype = Nullable.GetUnderlyingType(fieldInfo.Property.PropertyType);

                if (proptype is null) {
                    isNullable = false;
                    proptype = fieldInfo.Property.PropertyType;
                }

                // If the value is empty and the property is nullable then set the filter to null
                if (isNullable && item.Value.Count.Equals(0)) {
                    filter.Add(new Clause<T>(fieldInfo.Property.Name, EvalOp.Equals, null), FilterOp.And);
                    continue;
                }

                object typedVal;
                try {
                    var converter = TypeDescriptor.GetConverter(proptype);
                    typedVal = converter.ConvertFromString(item.Value);
                    filter.Add(new Clause<T>(fieldInfo.Property.Name, EvalOp.Equals, typedVal), FilterOp.And);
                } catch (NotSupportedException ex) {
                    log.LogDebug($"Type conversion for {fieldInfo.Property.Name} on type {typeof(T).FullName} failed. String value was '{item.Value}'", ex);
                    continue;
                }
            }

            return filter;
        }

        public string GetQuery(IFilter<T> filter) {
            var sb = new StringBuilder("?");
            foreach (var clause in filter) {
                if (clause is Clause<T> cl) {
                    sb.Append($"{cl.Property.Name}={cl.Value}&");
                } else {
                    log.LogDebug("Filter Groups are not supported by this service and will be ignored.");
                }
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }


    }
}

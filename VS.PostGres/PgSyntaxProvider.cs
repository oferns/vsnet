
namespace VS.PostGres {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using VS.Abstractions.Data;
    using VS.Abstractions.Data.Filtering;
    using VS.Abstractions.Data.Paging;
    using VS.Abstractions.Data.Sorting;
    using VS.Abstractions.Logging;

    public class PgSyntaxProvider<T> : IQuerySyntaxProvider<T> where T : class {
        private readonly IMetaData<T> metaData;
        private readonly ILog log;

        public PgSyntaxProvider(IMetaData<T> metaData, ILog log) {
            this.metaData = metaData ?? throw new ArgumentNullException(nameof(metaData));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public string Count(IFilter<T> filter, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);
            return $"SELECT COUNT(*) FROM {metaData.NativeName} {where}".Trim() + ";";
        }

        public string Delete(IFilter<T> filter, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);
            return $"DELETE FROM {metaData.NativeName} {where}".Trim() + " RETURNING *;";
        }

        public string Insert(IEnumerable<T> entities, out IDictionary<string, object> parameters) {

            parameters = new Dictionary<string, object>();
            var sql = new StringBuilder($"INSERT INTO {metaData.NativeName} ({string.Join(", ", metaData.Fields.Select(f => f.NativeName))}) VALUES ");

            for (var x = 0; x < entities.Count(); x++) {
                sql.Append("(");
                var entity = entities.ElementAt(x);

                foreach (var field in metaData.Fields) {
                    if (parameters.TryAdd($"{field.Property.Name}{x}", typeof(T).GetProperty(field.Property.Name).GetValue(entity))) {
                        sql.Append($"@{field.Property.Name}{x}, ");
                    }
                }

                sql.Remove(sql.Length - 2, 2); // Remove the last comma and space
                sql.Append("), ");
            }
            sql.Remove(sql.Length - 2, 2); // Remove the last comma and space
            
            return sql.ToString().Trim() + " RETURNING *;";
        }

        public string Select<A>(A args, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) where A : class {

            if (!metaData.ArgumentType.Equals(typeof(A))) {
                throw new Exception("Type mismatch");
            }

            var where = GenerateWhereClause(filter, out parameters);
            var orderBy = GenerateOrderBy(sorter);
            var fields = string.Join(", ", metaData.Fields.Select(f => f.NativeName)).TrimEnd();
            var tableexpression = $"{metaData.NativeName}(";
            foreach (var propinfo in args.GetType().GetProperties()) {
                parameters.TryAdd(propinfo.Name, propinfo.GetValue(args));
                tableexpression += $"@{propinfo.Name},";
            }
            tableexpression = tableexpression.Substring(0, tableexpression.Length - 1);
            tableexpression += ")";
            var limitoffset = pager == null ? string.Empty : $"LIMIT {pager.PageSize} OFFSET {pager.StartFrom}";
            return $"SELECT {fields} FROM {tableexpression} {where}".Trim() + $" {orderBy}".TrimEnd() + $" {limitoffset}".TrimEnd() + ";";
        }

        public string Select(IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);
            var orderBy = GenerateOrderBy(sorter);
            var fields = string.Join(", ", metaData.Fields.Select(f => f.NativeName)).TrimEnd();
            var limitoffset = pager == null ? string.Empty : $"LIMIT {pager.PageSize} OFFSET {pager.StartFrom}";

            return $"SELECT {fields} FROM {metaData.NativeName} {where}".TrimEnd() + $" {orderBy}".TrimEnd() + $" {limitoffset}".TrimEnd() + ";";
        }

        public string Select(IEnumerable<string> columns, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);
            var orderBy = GenerateOrderBy(sorter);
            var fields = string.Join(", ", metaData.Fields.Where(f => columns.Contains(f.Property.Name)).Select(f => f.NativeName)).TrimEnd();

            if (!fields.Any()) {
                log.LogDebug($"None of the requested properties are recognised. Returning all properties");

                fields = string.Join(", ", metaData.Fields.Select(f => f.NativeName));
            } else {
                var missingfields = columns.Where(c => !metaData.Fields.Any(m => m.Property.Name.Equals(c)));
                if (missingfields.Any()) {
                    log.LogDebug($"Select Property not recognized. The type is {typeof(T).Name}, the missing properties are {string.Join(", ", missingfields)}.");
                }
            }

            var limitoffset = pager == null ? string.Empty : $"LIMIT {pager.PageSize} OFFSET {pager.StartFrom}";

            return $"SELECT {fields} FROM {metaData.NativeName} {where}".Trim() + $" {orderBy}".TrimEnd() + $" {limitoffset}".TrimEnd() + ";";
        }

        public string Select<A>(A args, IEnumerable<string> columns, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) where A : class {
            if (!metaData.ArgumentType.Equals(typeof(A))) {
                throw new Exception("Type mismatch");
            }

            var where = GenerateWhereClause(filter, out parameters);
            var orderBy = GenerateOrderBy(sorter);
            var fields = string.Join(", ", metaData.Fields.Where(f => columns.Contains(f.Property.Name)).Select(f => f.NativeName)).TrimEnd();

            if (!fields.Any()) {
                log.LogDebug($"None of the requested properties are recognised. Returning all properties");

                fields = string.Join(", ", metaData.Fields.Select(f => f.NativeName));
            } else {
                var missingfields = columns.Where(c => !metaData.Fields.Any(m => m.Property.Name.Equals(c)));
                if (missingfields.Any()) {
                    log.LogDebug($"Select Property not recognized. The type is {typeof(T).Name}, the missing properties are {string.Join(", ", missingfields)}.");
                }
            }

            var tableexpression = $"{metaData.NativeName}(";
            foreach (var propinfo in args.GetType().GetProperties()) {
                parameters.TryAdd(propinfo.Name, propinfo.GetValue(args));
                tableexpression += $"@{propinfo.Name},";
            }
            tableexpression = tableexpression.Substring(0, tableexpression.Length - 1);
            tableexpression += ")";
            var limitoffset = pager == null ? string.Empty : $"LIMIT {pager.PageSize} OFFSET {pager.StartFrom}";
            return $"SELECT {fields} FROM {tableexpression} {where}".Trim() + $" {orderBy}".TrimEnd() + $" {limitoffset}".TrimEnd() + ";";
        }

        public string Update(IDictionary<string, object> properties, IFilter<T> filter, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);

            var fieldObjects = metaData.Fields.Where(f => properties.ContainsKey(f.Property.Name));
            var missingfields = properties.Where(c => !metaData.Fields.Any(m => m.Property.Name.Equals(c)));


            if (!fieldObjects.Any()) {
                throw new ApplicationException("There are no recognised fields to be updated.");
            }

            if (missingfields.Any()) {
                log.LogDebug($"Missing properties in update operation! {typeof(T).Name}, the missing properties are {string.Join(", ", missingfields)}.");
            }

            var props = string.Join(',', fieldObjects.Select(c => $"{c.NativeName} = @{c.Property.Name}"));

            foreach (var updateField in fieldObjects) {
                parameters.Add($"@{updateField.Property.Name}",
                    properties[updateField.Property.Name]);
            }

            return $"UPDATE {metaData.NativeName} SET {props} {where} RETURNING *;";
        }


        public string GenerateWhereClause(IFilter<T> filter, out IDictionary<string, object> parameters) {
            parameters = new Dictionary<string, object>();

            if (filter == null || filter.Count == 0) {
                return string.Empty;
            }

            // Begin
            var sb = new StringBuilder();
            var index = 0;
            foreach (var entry in filter) {
                if (index == 0) {
                    sb.Append("WHERE ");
                } else {
                    switch (entry.Op) {
                        default:
                        case FilterOp.And:
                            sb.Append("AND ");
                            break;
                        case FilterOp.Or:
                            sb.Append("OR ");
                            break;
                    }
                }
                sb.Append(GenerateClause(entry.Clause, index, index.ToString(), ref parameters));
                index++;
            }
            return sb.ToString();
        }

        public string GenerateOrderBy(ISorter<T> sorter) {
            if (sorter == null) {
                return string.Empty;
            }

            var sb = new StringBuilder("ORDER BY ");

            foreach (var clause in sorter) {
                var key = metaData.Fields.Where(f => f.Property.Name.Equals(clause.Key)).SingleOrDefault();
                if (key.NativeName is null) {
                    log.LogDebug($"OrderBy Property not recognized. This should not happen due to type checking. The type is {typeof(T).Name}, the midssing property is {clause.Key}.");
                    continue;
                }

                sb.Append(key.NativeName);
                sb.Append(clause.Value ? " ASC," : " DESC,");
            }


            return sb.ToString().TrimEnd(',').Trim();
        }

        private string GenerateClause(IClause<T> clause, int level, string tag, ref IDictionary<string, object> parameters) {
            var sb = new StringBuilder();

            if (clause is IFilter<T> group) {
                var sublevel = 0;
                sb.Append("(");
                foreach (var subclause in group) {
                    sb.Append(GenerateClause(clause, sublevel, $"{tag}_{level}", ref parameters));
                    sublevel++;
                }
                sb.Append(")");
                return sb.ToString();
            } else if (clause is Clause<T> expression) {
                var cl = ClauseToString(expression, $"{tag}_{level}", ref parameters);
                sb.Append(cl);
                return sb.ToString();
            }


            return sb.ToString();
        }
        private string ClauseToString(Clause<T> clause, string tag, ref IDictionary<string, object> parameterObject) {
            var sb = new StringBuilder();

            var fieldName = metaData.Fields.Where(f => f.Property.Name.Equals(clause.Property.Name)).Single();
            var parameterName = $"{clause.Property.Name}{tag}";

            switch (clause.EvalOp) {
                default:
                case EvalOp.Equals:
                    if (clause.Value is null) {
                        sb.Append($"{fieldName.NativeName} IS NULL");
                    } else {
                        sb.Append($"{fieldName.NativeName} = @{parameterName}");
                    }
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.NotEqual:
                    if (clause.Value is null) {
                        sb.Append($"{fieldName.NativeName} IS NOT NULL");
                    } else {
                        sb.Append($"{fieldName.NativeName} <> @{parameterName}");
                    }
                    parameterObject.Add(parameterName, clause.Value);
                    break;

                case EvalOp.GreaterThan:
                    sb.Append($"{fieldName.NativeName} > @{parameterName}");
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.LessThan:
                    sb.Append($"{fieldName.NativeName} < @{parameterName}");
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.EqualOrGreaterThan:
                    sb.Append($"{fieldName.NativeName} >= @{parameterName}");
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.EqualOrLessThan:
                    sb.Append($"{fieldName.NativeName} <= @{parameterName}");
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.StartsWith:
                    sb.Append($"{fieldName.NativeName} ILIKE @{parameterName}");
                    parameterObject.Add(parameterName, $"{clause.Value}%");
                    break;
                case EvalOp.EndsWith:
                    sb.Append($"{fieldName.NativeName} ILIKE @{parameterName}");
                    parameterObject.Add(parameterName, $"%{clause.Value}");
                    break;
                case EvalOp.Contains:
                    if (clause.Value is IPAddress || clause.Value is ValueTuple<IPAddress, int>) {
                        sb.Append($"{fieldName.NativeName} >>= inet '{clause.Value.ToString()}' ");
                    } else {
                        sb.Append($"{fieldName.NativeName} ILIKE @{parameterName}");
                        parameterObject.Add(parameterName, $"%{clause.Value}%");
                    }
                    break;
            }
            return sb.ToString();
        }


    }
}
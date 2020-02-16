namespace VS.PostGres {
    using System;
    using System.Collections.Generic;
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

        public string Count<A>(A args, IFilter<T> filter, out IDictionary<string, object> parameters) {
            if (!(metaData.ArgumentType?.Equals(typeof(A)) ?? false)) {
                throw new Exception("Type mismatch");
            }

            var where = GenerateWhereClause(filter, out parameters);

            var tableexpression = $"{metaData.NativeName}(";
            foreach (var propinfo in args.GetType().GetProperties()) {
                parameters.TryAdd(propinfo.Name, propinfo.GetValue(args));
                tableexpression += $"@{propinfo.Name},";
            }
            tableexpression = tableexpression.Substring(0, tableexpression.Length - 1);
            tableexpression += ")";

            return $"SELECT COUNT(*) FROM {tableexpression} {where}".Trim() + ";";

        }

        public string Delete(IFilter<T> filter, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);
            return $"DELETE FROM {metaData.NativeName} {where}".Trim() + " RETURNING *;";
        }

        public string Insert(IEnumerable<T> entities, out IDictionary<string, object> parameters) {

            parameters = new Dictionary<string, object>();
            var sql = new StringBuilder($"INSERT INTO {metaData.NativeName} (");

            foreach (var field in metaData.Fields) {
                sql.Append(field.NativeName).Append(", ");
            }

            sql.Remove(sql.Length - 2, 2);

            sql.Append(") VALUES ");

            var index = 0;
            foreach (var entity in entities) {

                sql.Append("(");
                foreach (var field in metaData.Fields) {
                    if (parameters.TryAdd($"{field.Property.Name}{index}", typeof(T).GetProperty(field.Property.Name).GetValue(entity))) {
                        sql.Append($"@{field.Property.Name}{index}, ");
                    }
                }

                sql.Remove(sql.Length - 2, 2); // Remove the last comma and space
                sql.Append("), ");
                index++;
            }
            sql.Remove(sql.Length - 2, 2); // Remove the last comma and space

            return sql.ToString().Trim() + " RETURNING *;";
        }

        public string Select<A>(A args, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) where A : class {

            if (!(metaData.ArgumentType?.Equals(typeof(A)) ?? false)) {
                throw new Exception("Type mismatch");
            }

            var where = GenerateWhereClause(filter, out parameters);
            var orderBy = GenerateOrderBy(sorter);
            var fields = new List<string>();

            foreach (var field in metaData.Fields) {
                fields.Add(field.NativeName);
            }

            
            var tableexpression = $"{metaData.NativeName}(";
            foreach (var propinfo in args.GetType().GetProperties()) {
                parameters.TryAdd(propinfo.Name, propinfo.GetValue(args));
                tableexpression += $"@{propinfo.Name},";
            }
            tableexpression = tableexpression.Substring(0, tableexpression.Length - 1);
            tableexpression += ")";
            var limitoffset = pager == null ? string.Empty : $"LIMIT {pager.PageSize} OFFSET {pager.StartFrom}";
            return $"SELECT {string.Join(", ", fields)} FROM {tableexpression} {where}".Trim() + $" {orderBy}".TrimEnd() + $" {limitoffset}".TrimEnd() + ";";
        }

        public string Select(IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);
            var orderBy = GenerateOrderBy(sorter);
            var fields = new List<string>();

            foreach (var field in metaData.Fields) {
                fields.Add(field.NativeName);
            }

            var limitoffset = pager == null ? string.Empty : $"LIMIT {pager.PageSize} OFFSET {pager.StartFrom}";

            return $"SELECT {string.Join(", ", fields)} FROM {metaData.NativeName} {where}".TrimEnd() + $" {orderBy}".TrimEnd() + $" {limitoffset}".TrimEnd() + ";";
        }

        public string Select(string[] columns, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);
            var orderBy = GenerateOrderBy(sorter);

            var nativefields = new List<string>();
            var missingfields = new List<string>();

            foreach (var field in metaData.Fields) {
                foreach (var column in columns) {
                    if (column.Equals(field.Property.Name, StringComparison.OrdinalIgnoreCase)
                       || column.Equals(field.NativeName, StringComparison.OrdinalIgnoreCase)) {
                        nativefields.Add(field.NativeName);
                    } else {
                        missingfields.Add(column);
                    }
                }
            }

            if (nativefields.Count.Equals(0)) {
                log.LogDebug($"None of the requested properties are recognised. Returning all properties");
                foreach (var field in metaData.Fields) {
                    nativefields.Add(field.NativeName);
                }
            }

            if (missingfields.Count > 0) {
                log.LogDebug($"Select Property not recognized. The type is {typeof(T).Name}, the missing properties are {string.Join(", ", missingfields)}.");
            }

            var limitoffset = pager == null ? string.Empty : $"LIMIT {pager.PageSize} OFFSET {pager.StartFrom}";

            return $"SELECT {string.Join(", ", nativefields)} FROM {metaData.NativeName} {where}".Trim() + $" {orderBy}".TrimEnd() + $" {limitoffset}".TrimEnd() + ";";
        }

        public string Select<A>(A args, string[] columns, IFilter<T> filter, ISorter<T> sorter, IPager pager, out IDictionary<string, object> parameters) where A : class {
            if (!metaData.ArgumentType.Equals(typeof(A))) {
                throw new Exception("Type mismatch");
            }

            var where = GenerateWhereClause(filter, out parameters);
            var orderBy = GenerateOrderBy(sorter);

            var nativefields = new List<string>();
            var missingfields = new List<string>();

            foreach (var field in metaData.Fields) {
                foreach (var column in columns) {
                    if (column.Equals(field.Property.Name, StringComparison.OrdinalIgnoreCase)
                       || column.Equals(field.NativeName, StringComparison.OrdinalIgnoreCase)) {
                        nativefields.Add(field.NativeName);
                    } else {
                        missingfields.Add(column);
                    }
                }
            }

            if (nativefields.Count.Equals(0)) {
                log.LogDebug($"None of the requested properties are recognised. Returning all properties");
                foreach (var field in metaData.Fields) {
                    nativefields.Add(field.NativeName);
                }
            }

            if (missingfields.Count > 0) {
                log.LogDebug($"Select Property not recognized. The type is {typeof(T).Name}, the missing properties are {string.Join(", ", missingfields)}.");
            }

            var tableexpression = $"{metaData.NativeName}(";
            foreach (var propinfo in args.GetType().GetProperties()) {
                parameters.TryAdd(propinfo.Name, propinfo.GetValue(args));
                tableexpression += $"@{propinfo.Name},";
            }
            tableexpression = tableexpression.Substring(0, tableexpression.Length - 1);
            tableexpression += ")";
            var limitoffset = pager == null ? string.Empty : $"LIMIT {pager.PageSize} OFFSET {pager.StartFrom}";
            return $"SELECT {string.Join(", ",nativefields)} FROM {tableexpression} {where}".Trim() + $" {orderBy}".TrimEnd() + $" {limitoffset}".TrimEnd() + ";";
        }

        public string Update(IDictionary<string, object> properties, IFilter<T> filter, out IDictionary<string, object> parameters) {
            var where = GenerateWhereClause(filter, out parameters);

            var fieldObjects = new List<DbFieldInfo>();
            var missingfields = new List<string>();

            foreach (var field in metaData.Fields) {
                foreach (var property in properties) {
                    if (property.Key.Equals(field.Property.Name, StringComparison.OrdinalIgnoreCase)
                       || property.Key.Equals(field.NativeName, StringComparison.OrdinalIgnoreCase)) {
                        fieldObjects.Add(field);
                    } else {
                        missingfields.Add(property.Key);
                    }
                }
            }


            if (fieldObjects.Count.Equals(0)) {
                throw new ApplicationException("There are no recognised fields to be updated.");
            }

            if (missingfields.Count > 0) {
                log.LogDebug($"Missing properties in update operation! {typeof(T).Name}, the missing properties are {string.Join(", ", missingfields)}.");
            }

            var sb = new StringBuilder();
            foreach (var field in fieldObjects) {
                sb.Append($"{field.NativeName} = @{field.Property.Name},");
            }
            sb.Remove(sb.Length - 1, 1);

            foreach (var updateField in fieldObjects) {
                parameters.Add($"@{updateField.Property.Name}",
                    properties[updateField.Property.Name]);
            }

            return $"UPDATE {metaData.NativeName} SET {sb.ToString()} {where} RETURNING *;";
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
                var found = false;
                foreach (var field in metaData.Fields) {
                    if (field.Property.Name.Equals(clause.Key, StringComparison.OrdinalIgnoreCase)
                        || field.NativeName.Equals(clause.Key, StringComparison.OrdinalIgnoreCase)) {
                        sb.Append(field.NativeName).Append(clause.Value ? " ASC," : " DESC,");
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    log.LogDebug($"OrderBy Property not recognized. This should not happen due to type checking. The type is {typeof(T).Name}, the midssing property is {clause.Key}.");
                }
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

            var fieldinfo = default(DbFieldInfo);

            foreach (var field in metaData.Fields) {
                if (field.Property.Name.Equals(clause.Property.Name)) {
                    fieldinfo = field;
                    break;
                }
            }

            var parameterName = $"{clause.Property.Name}{tag}";

            switch (clause.EvalOp) {
                default:
                case EvalOp.Equals:
                    if (clause.Value is null) {
                        sb.Append($"{fieldinfo.NativeName} IS NULL");
                    } else {
                        sb.Append($"{fieldinfo.NativeName} = @{parameterName}");
                    }
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.NotEqual:
                    if (clause.Value is null) {
                        sb.Append($"{fieldinfo.NativeName} IS NOT NULL");
                    } else {
                        sb.Append($"{fieldinfo.NativeName} <> @{parameterName}");
                    }
                    parameterObject.Add(parameterName, clause.Value);
                    break;

                case EvalOp.GreaterThan:
                    sb.Append($"{fieldinfo.NativeName} > @{parameterName}");
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.LessThan:
                    sb.Append($"{fieldinfo.NativeName} < @{parameterName}");
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.EqualOrGreaterThan:
                    sb.Append($"{fieldinfo.NativeName} >= @{parameterName}");
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.EqualOrLessThan:
                    sb.Append($"{fieldinfo.NativeName} <= @{parameterName}");
                    parameterObject.Add(parameterName, clause.Value);
                    break;
                case EvalOp.StartsWith:
                    sb.Append($"{fieldinfo.NativeName} ILIKE @{parameterName}");
                    parameterObject.Add(parameterName, $"{clause.Value}%");
                    break;
                case EvalOp.EndsWith:
                    sb.Append($"{fieldinfo.NativeName} ILIKE @{parameterName}");
                    parameterObject.Add(parameterName, $"%{clause.Value}");
                    break;
                case EvalOp.Contains:
                    if (clause.Value is IPAddress || clause.Value is ValueTuple<IPAddress, int>) {
                        sb.Append($"{fieldinfo.NativeName} >>= inet '{clause.Value.ToString()}' ");
                    } else {
                        sb.Append($"{fieldinfo.NativeName} ILIKE @{parameterName}");
                        parameterObject.Add(parameterName, $"%{clause.Value}%");
                    }
                    break;
            }
            return sb.ToString();
        }

    
    }
}
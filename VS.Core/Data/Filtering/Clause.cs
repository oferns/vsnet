
namespace VS.Core.Data.Filtering {
    using System;
    using System.Reflection;
    using VS.Core.Data.Abstractions;

    public class Clause<T> : IClause<T> where T : class {
        public Clause(string property, EvalOp evalOp, object value) {
            Property = typeof(T).GetProperty(property, BindingFlags.Public | BindingFlags.Instance)
                ?? throw new ArgumentNullException(nameof(property), $"The property {property} was not found on type {typeof(T).GetType().FullName}");
            EvalOp = evalOp;
            Value = value; // TODO: Type check this against the property type
        }

        public PropertyInfo Property { get; private set; }
        public EvalOp EvalOp { get; private set; }
        public object Value { get; private set; }

        public override bool Equals(object obj) {
            if (!(obj is Clause<T> clause)) {
                return false;
            }
            return clause.GetHashCode().Equals(this.GetHashCode());
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 37;
                hash = hash * 23 + Value.GetHashCode();
                hash += (int)EvalOp;
                hash = hash * 23 + (Property.GetHashCode());
                return hash;
            }
        }
    }
}

﻿namespace VS.Abstractions.Data.Filtering {
    using System.Collections.Generic;


    /// <summary>
    /// A FIFO collection of filter clauses <see cref="ClauseEntry"/>
    /// with a preceeding operator
    /// </summary>
    /// <typeparam name="T">The type representing a database entity</typeparam>
    public class Filter<T> : Queue<ClauseEntry<T>>, IFilter<T> where T : class {

        public bool IsReadOnly => true;

        public void Add(ClauseEntry<T> item) {
            Enqueue(item);
        }

        public void Add(IClause<T> clause, FilterOp op = FilterOp.And) {
            Enqueue(new ClauseEntry<T>(op, clause));
        }

        public bool Remove(ClauseEntry<T> item) {
            return false;
        }

        public override bool Equals(object obj) {
            if (!(obj is IFilter<T> clause)) {
                return false;
            }
            return GetHashCode().Equals(clause.GetHashCode());
        }

        public override int GetHashCode() {
            unchecked {
                var code = 27;
                foreach (var f in this) {
                    code += (int)f.Op;
                    code ^= f.Clause.GetHashCode();
                }
                return code;
            }
        }
    }
}
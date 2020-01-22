namespace VS.Abstractions.Data {

    using System;
    using System.Collections.Generic;

    public interface IMetaData<T> where T : class {

        string NativeName { get; }

        Type ReturnType { get; }

        Type ArgumentType { get; }

        IEnumerable<Field> Fields { get; }

        IEnumerable<int> PrimaryKeys { get;}

        IDictionary<object, IEnumerable<int>> ForeignKeys { get;  }

        IDictionary<ValueTuple<string, bool>, IEnumerable<int>> Indices { get; }

    }
}

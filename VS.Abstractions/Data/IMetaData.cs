namespace VS.Abstractions.Data {

    using System;
    using System.Collections.Generic;

    public interface IMetaData<T> where T : class {

        string NativeName { get; }

        Type ReturnType { get; }

        Type ArgumentType { get; }

        IList<DbFieldInfo> Fields { get; }

        IList<int> PrimaryKeys { get;}
        
        IList<ForeignKeyInfo> ForeignKeys { get;  }

        IList<IndexInfo> Indices { get; }

    }
}

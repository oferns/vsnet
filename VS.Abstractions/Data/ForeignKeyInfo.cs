namespace VS.Abstractions.Data {

    using System;
    using System.Collections.Generic;

    public struct ForeignKeyInfo {

        public ForeignKeyInfo(IDictionary<int, string> columnToProperty, Type foreignType) {
            ColumnToProperty = columnToProperty;
            ForeignType = foreignType;
        }

        public IDictionary<int, string> ColumnToProperty { get; }
        public Type ForeignType { get; }
    }
}

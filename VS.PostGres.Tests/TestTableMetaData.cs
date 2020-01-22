namespace VS.PostGres.Tests {

    using System;
    using System.Collections.Generic;
    using VS.Abstractions.Data;

    public struct TestTableMetadata : IMetaData<TestTable> {

        public string NativeName => "test.test_table";
        public Type ReturnType => typeof(TestTable);
        public Type ArgumentType => default;

        public IEnumerable<int> PrimaryKeys => new[] { 0 };
        public IDictionary<object, IEnumerable<int>> ForeignKeys => new Dictionary<object, IEnumerable<int>>();

        public IDictionary<ValueTuple<string, bool>, IEnumerable<int>> Indices => new Dictionary<ValueTuple<string, bool>, IEnumerable<int>> {
            {  ValueTuple.Create("ix_culture_specific_culture", true) , new [] { 1 } }
        };

        public IEnumerable<Field> Fields => new[] {
            new Field {
                    Ordinal = 0,
                    Property = typeof(TestTable).GetProperty("IntProp"),
                    NativeName = "int_prop",
                    DbType = 11, // DbType.Int32
                    DbTypeName = "int(11)",
                    DefaultExpression = null,
                    Generated = true
            },
            new Field {
                    Ordinal = 1,
                    Property = typeof(TestTable).GetProperty("StringProp"),
                    NativeName = "string_prop",
                    DbType = 0x10, // DbType.String
                    DbTypeName = "character varying(24)",
                    DefaultExpression = null,
                    Generated = false,
                    ReadOnly = false,
            } ,
            new Field {
                    Ordinal = 2,
                    NativeName = "english_name",
                    DbType = 0x10, // DbType.String
                    DbTypeName = "character varying(255)",
                    DefaultExpression = null,
                    Generated = false,
                    ReadOnly = false
            } ,
            new Field {
                    Ordinal = 3,
                    NativeName = "local_name",
                    DbType = 0x10, // DbType.String
                    DbTypeName = "character varying(255)",
                    DefaultExpression = null,
                    Generated = false,
                    ReadOnly = false,
            }
        };
    }
}

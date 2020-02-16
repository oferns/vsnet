namespace VS.PostGres.Tests {

    using System;
    using System.Collections.Generic;
    using VS.Abstractions.Data;

    public struct TestTableMetadata : IMetaData<TestTable> {

        public string NativeName => "test.test_table";
        public Type ReturnType => typeof(TestTable);
        public Type ArgumentType => default;

        public IList<int> PrimaryKeys => new[] { 0 };
        public IDictionary<object, IEnumerable<int>> ForeignKeys => new Dictionary<object, IEnumerable<int>>();

        public IList<IndexInfo> Indices => new List<IndexInfo> {
              new IndexInfo ("ix_culture_specific_culture",true , new [] { 1 })
        };

        public IList<DbFieldInfo> Fields => new[] {
            new DbFieldInfo(
                    0,
                    typeof(TestTable).GetProperty("IntProp"),
                    "int_prop",
                    "int(11)",
                    11, // DbType.Int32                    
                    true,
                    true,
                    null                                
            ),
            new DbFieldInfo(
                    1,
                    typeof(TestTable).GetProperty("StringProp"),
                    "string_prop",
                    "character varying(24)",
                    0x10, // DbType.String
                    false,
                    false,
                    null
            ),
            new DbFieldInfo(
                    2,
                    typeof(TestTable).GetProperty("FloatProp"),
                    "float_prop",
                    "decimal",
                    6, // DbType.Float                    
                    false,
                    false,
                    null
            ),
            new DbFieldInfo(
                    3,
                    typeof(TestTable).GetProperty("DoubleProp"),
                    "double_prop",
                    "decimal",
                    6, // DbType.Float
                    false,
                    false,
                    null
            ),
            new DbFieldInfo (
                    4,
                    typeof(TestTable).GetProperty("DecimalProp"),
                    "decimal_prop",
                    "character varying(255)",
                    0x10, // DbType.String                    
                    false,
                    false,
                    null
            )
        };
    }
}
namespace VS.Data.PostGres.App.Meta {

    using System;
    using System.Collections.Generic;
    using System.Data;
    using VS.Abstractions.Data;

    public class CultureMetadata : IMetaData<Culture> {
        public string NativeName => "app.culture";
        public Type ReturnType => typeof(Culture);
        public Type ArgumentType => default;

//        public IList<int> PrimaryKeys => new[] { 0 };
        public IList<int> PrimaryKeys => Array.Empty<int>();


        public IList<ForeignKeyInfo> ForeignKeys => new[] { new ForeignKeyInfo(new Dictionary<int, string> { { 1, "Id" }, { 2, "Name" } }, typeof(Culture)), new ForeignKeyInfo(new Dictionary<int, string> { { 1, "Id" }, { 2, "Name" } }, typeof(Culture)) };

        public IList<IndexInfo> Indices => new [] { new IndexInfo("ix_culture_specific_culture", true, new int[] { 1 }) };

        public IList<DbFieldInfo> Fields => new[] {

             new DbFieldInfo(
                    0,
                    typeof(Culture).GetProperty("Code"),
                    "code",
                    "int(11)",
                    (int) DbType.Int32,
                    true,
                    true,
                    null
            ),
             new DbFieldInfo(
                    1,
                    typeof(Culture).GetProperty("SpecificCulture"),
                    "specific_culture",
                    "character varying(24)",
                    (int) DbType.String,
                    false,
                    false,
                    null
            ),
             new DbFieldInfo(
                    2,
                    typeof(Culture).GetProperty("EnglishName"),
                    "english_name",
                    "character varying(255)",
                    (int) DbType.String,
                    false,
                    false,
                    null
            ),
            new DbFieldInfo(
                    3,
                    typeof(Culture).GetProperty("LocalName"),
                    "local_name",
                    "character varying(255)",
                    (int) DbType.String,
                    false,
                    false,
                    null
            )
        };
    }
}
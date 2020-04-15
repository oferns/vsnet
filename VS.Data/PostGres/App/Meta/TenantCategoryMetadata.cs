namespace VS.Data.PostGres.App.Meta {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using VS.Abstractions.Data;

    public class TenantCategoryMetadata : IMetaData<TenantCategory> {

        public string NativeName => "app.tenant_categories";
        public Type ReturnType => typeof(TenantCategory);
        public Type ArgumentType => default;


        public IList<int> PrimaryKeys => new[] { 0 };
        public IList<ForeignKeyInfo> ForeignKeys => Array.Empty<ForeignKeyInfo>();

        public IList<IndexInfo> Indices => new List<IndexInfo> { new IndexInfo("ix_stamp_unique", true, new int[] { 1 }) };

        public IList<DbFieldInfo> Fields => new[] {

             new DbFieldInfo(
                    0,
                    typeof(SecurityStamp).GetProperty("Parent"),
                    "parent",
                    "int(11)",
                    (int) DbType.Int32,
                    true,
                    true,
                    null
            )
        };
    }
}

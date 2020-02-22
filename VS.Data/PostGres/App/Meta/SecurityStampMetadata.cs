namespace VS.Data.PostGres.App.Meta {

    using System;
    using System.Collections.Generic;
    using System.Data;
    using VS.Abstractions.Data;

    public class SecurityStampMetadata : IMetaData<SecurityStamp> {
        public string NativeName => "app.security_stamp";
        public Type ReturnType => typeof(SecurityStamp);
        public Type ArgumentType => default;

        public IList<int> PrimaryKeys => new[] { 0 };
        public IDictionary<Type, IEnumerable<int>> ForeignKeys => new Dictionary<Type, IEnumerable<int>>();

        public IList<IndexInfo> Indices => new List<IndexInfo> { new IndexInfo("ix_stamp_unique", true, new int[] { 1 }) };

        public IList<DbFieldInfo> Fields => new[] {

             new DbFieldInfo(
                    0,
                    typeof(SecurityStamp).GetProperty("Stamp"),
                    "stamp",
                    "uuid",
                    (int) DbType.Guid,
                    true,
                    true,
                    null
            )
        };
    }
}
namespace VS.Data.PostGres.Book.Meta {

    using System;
    using System.Collections.Generic;
    using System.Data;
    using VS.Abstractions.Data;

    public sealed class VwOrderMetadata : IMetaData<VwOrder> {

        public string NativeName => "book.vw_order";
        public Type ReturnType => typeof(VwOrder);
        public Type ArgumentType => default;

        public IList<int> PrimaryKeys => Array.Empty<int>();
        public IList<ForeignKeyInfo> ForeignKeys => Array.Empty<ForeignKeyInfo>();

        public IList<IndexInfo> Indices => Array.Empty<IndexInfo>();

        public IList<DbFieldInfo> Fields => new[] {

             new DbFieldInfo(
                    0,
                    typeof(VwOrder).GetProperty("OrderId"),
                    "order_id",
                    "int(11)",
                    (int) DbType.Int32,
                    true,
                    false,
                    null
            ),
            new DbFieldInfo(
                    1,
                    typeof(VwOrder).GetProperty("Host"),
                    "host",
                    "character varying(253)",
                    (int) DbType.String,
                    true,
                    false,
                    null
            ),
            new DbFieldInfo(
                    2,
                    typeof(VwOrder).GetProperty("UserId"),
                    "user_id",
                    "int(11)",
                    (int) DbType.Int32,
                    true,
                    false,
                    null
            ),
            new DbFieldInfo(
                    2,
                    typeof(VwOrder).GetProperty("Amount"),
                    "amount",
                    "decimal(10,2)",
                    (int) DbType.Decimal,
                    true,
                    false,
                    null
            ),

        };
    }
}


namespace VS.Data.PostGres.Book.Meta {
    
    using System;
    using System.Collections.Generic;
    using System.Data;
    using VS.Abstractions.Data;

    public sealed class OrderItemMetadata : IMetaData<OrderItem> {

        public string NativeName => "book.order_item";
        public Type ReturnType => typeof(OrderItem);
        public Type ArgumentType => default;
        
        public IList<int> PrimaryKeys => new[] { 0 };

        public IDictionary<Type, IEnumerable<int>> ForeignKeys => new Dictionary<Type, IEnumerable<int>>();

        public IList<IndexInfo> Indices => Array.Empty<IndexInfo>();

        public IList<DbFieldInfo> Fields => new[] {

             new DbFieldInfo(
                    0,
                    typeof(OrderItem).GetProperty("Id"),
                    "id",
                    "int(11)",
                    (int) DbType.Int32,
                    true,
                    false,
                    null
            )
        };

    }
}

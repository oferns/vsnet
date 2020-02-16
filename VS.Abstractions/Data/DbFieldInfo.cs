namespace VS.Abstractions.Data {

    using System.Reflection;

    public struct DbFieldInfo {

        public DbFieldInfo(int ordinal, PropertyInfo property, string nativeName, string dbTypeName, int dbType, bool generated, bool readOnly, string defaultExpression) {
            Ordinal = ordinal;
            Property = property;
            NativeName = nativeName;
            DbTypeName = dbTypeName;
            DbType = dbType;
            Generated = generated;
            ReadOnly = readOnly;
            DefaultExpression = defaultExpression;
        }

        public int Ordinal { get;}
        public PropertyInfo Property { get; }        
        public string NativeName { get; }
        public string DbTypeName { get; }
        public int DbType { get; }
        public bool Generated { get; }        
        public bool ReadOnly { get; }
        public string DefaultExpression { get; }
        
    }
}
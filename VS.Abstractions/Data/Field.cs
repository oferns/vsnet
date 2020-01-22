namespace VS.Abstractions.Data {
    using System.Reflection;

    public struct Field {
        public int Ordinal { get; set; }
        public PropertyInfo Property { get; set; }        
        public string NativeName { get; set; }
        public string DbTypeName { get; set; }
        public int DbType { get; set; }
        public bool Generated { get; set; }        
        public bool ReadOnly { get; set; }
        public string DefaultExpression { get; set; }
        
    }
}

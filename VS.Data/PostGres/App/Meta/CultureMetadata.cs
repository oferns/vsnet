namespace VS.Data.PostGres.App.Meta {

    using System;
    using System.Collections.Generic;
    using System.Data;
    using VS.Abstractions.Data;

    public struct CultureMetadata : IMetaData<Culture> {
        public string NativeName => "app.culture";
        public Type ReturnType => typeof(Culture);
        public Type ArgumentType => default;

        public IEnumerable<int> PrimaryKeys => new[] { 0 };
        public IDictionary<object, IEnumerable<int>> ForeignKeys => new Dictionary<object, IEnumerable<int>>();

        public IDictionary<ValueTuple<string, bool>, IEnumerable<int>> Indices => new Dictionary<ValueTuple<string, bool>, IEnumerable<int>> {
            { ("ix_culture_specific_culture", true), new [] { 1 } }
        };

        public IEnumerable<Field> Fields => new[] { 
            new Field {
                    Ordinal = 0,
                    Property = typeof(Culture).GetProperty("Code"),
                    NativeName = "code",
                    DbType = (int)DbType.String,
                    DbTypeName = "character varying(24)",
                    DefaultExpression = null,                    
                    Generated = false,
                    ReadOnly = false,
            },
            new Field {
                    Ordinal = 1,
                    Property = typeof(Culture).GetProperty("SpecificCulture"),
                    NativeName = "specific_culture",
                    DbType = (int)DbType.String,
                    DbTypeName = "character varying(24)",
                    DefaultExpression = null,                    
                    Generated = false,
                    ReadOnly = false                    
            } ,
            new Field {
                    Ordinal = 2,
                    Property = typeof(Culture).GetProperty("EnglishName"),
                    NativeName = "english_name",
                    DbType = (int)DbType.String,
                    DbTypeName = "character varying(255)",
                    DefaultExpression = null,                    
                    Generated = false,
                    ReadOnly = false                    
            } ,
            new Field {
                    Ordinal = 3,
                    Property = typeof(Culture).GetProperty("LocalName"),
                    NativeName = "local_name",
                    DbType = (int)DbType.String,
                    DbTypeName = "character varying(255)",
                    DefaultExpression = null,                    
                    Generated = false,
                    ReadOnly = false
            }
        };        
    }
}
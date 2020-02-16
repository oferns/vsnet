namespace VS.Abstractions.Data {

    using System.Collections.Generic;

    public struct IndexInfo {

        public IndexInfo(string nativeName, bool unique, IEnumerable<int> fields) {
            NativeName = nativeName;
            Unique = unique;
            Fields = fields;
        }

        public string NativeName { get; }

        public bool Unique { get;  }

        public IEnumerable<int> Fields { get;  }

    }
}
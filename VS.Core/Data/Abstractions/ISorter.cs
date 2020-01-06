namespace VS.Core.Data.Abstractions {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISorter<T>: IEnumerable<KeyValuePair<string,bool>> where T : class {


        void Add(KeyValuePair<string, bool> column);

        void Clear();


    }
}

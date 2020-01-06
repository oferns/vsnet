namespace VS.Core.Data.Sorting {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Core.Data.Abstractions;


    /// <summary>
    /// Simple typed list to hold sorting options
    /// </summary>
    /// <typeparam name="T">The type you are sorting</typeparam>
    public class Sorter<T> : List<KeyValuePair<string, bool>>, ISorter<T> where T : class {
        
    }
}

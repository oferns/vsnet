namespace VS.Abstractions.Data.Sorting {

    using System;
    using System.Collections.Generic;
    using System.Text;


    /// <summary>
    /// Simple typed list to hold sorting options
    /// </summary>
    /// <typeparam name="T">The type you are sorting</typeparam>
    public class Sorter<T> : Dictionary<string, bool>, ISorter<T> where T : class {

    }
}

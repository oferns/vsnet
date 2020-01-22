namespace VS.Abstractions.Data.Filtering {


    /// <summary>
    /// Represents an entry in an <see cref="IFilter" />
    /// Can be a single entry or an array of entries with an operator
    /// </summary>
    /// <typeparam name="T">T must be a class and should represent a database entity structure (ie a table)</typeparam>
    public interface IClause<T> where T : class  { }
}
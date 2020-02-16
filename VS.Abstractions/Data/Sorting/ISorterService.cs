namespace VS.Abstractions.Data.Sorting {

    public interface ISorterService<T> where T : class  {

        ISorter<T> GetSorter();

    }
}

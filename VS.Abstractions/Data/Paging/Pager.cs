namespace VS.Abstractions.Data.Paging {
    public class Pager : IPager {

        public Pager(int startFrom, int pageSize) {
            StartFrom = startFrom;
            PageSize = pageSize;
        }
        
        public int StartFrom { get; }
        public int PageSize { get; }
    }
}

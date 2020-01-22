namespace VS.Abstractions.Data.Paging {
    public interface IPager {

        int StartFrom { get; }
        int PageSize { get; }

    }
}

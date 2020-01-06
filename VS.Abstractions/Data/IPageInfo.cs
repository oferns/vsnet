namespace VS.Abstractions.Data {
    public interface IPageInfo {

        int StartFrom { get; }
        int PageSize { get; }
        int Total { get; }

        int PageCount { get; }
        int CurrentPage { get; }
    }
}

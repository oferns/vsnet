namespace VS.Abstractions.Data.Paging {

    using System;
    using System.Collections.Generic;
    using VS.Abstractions.Data;

    public class PagedList<T> : List<T>, IPageInfo where T : class {

        public PagedList(IEnumerable<T> source, int startFrom, int pageSize, int total) : base(source) {
            this.StartFrom = startFrom;
            this.PageSize = pageSize;
            this.Total = total;
        }
        public int StartFrom { get; private set; }
        public int PageSize { get; private set; }
        public int Total { get; private set; }

        public int PageCount {
            get {
                return PageSize == 0 ? 0 : (int)Math.Ceiling(((decimal)Total) / ((decimal)PageSize));
            }
        }

        public int CurrentPage {
            get {
                return PageCount == 0 || StartFrom < PageSize ? 1 :  // If StartFrom is on the first page
                    StartFrom >= (Total - PageSize) ? PageCount :  // If startFrom is on the last page                     
                    (int)Math.Max(Math.Ceiling((decimal)(StartFrom + 1) / Math.Max(PageSize, 1)), 1);
            }
        }
    }
}
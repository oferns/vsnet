namespace VS.Mvc._Services {

    using Microsoft.AspNetCore.Http;
    using VS.Abstractions.Data.Paging;

    public class QueryStringPager : IPager {
        private readonly IHttpContextAccessor contextAccessor;

        public QueryStringPager(IHttpContextAccessor contextAccessor) {
            this.contextAccessor = contextAccessor ?? throw new System.ArgumentNullException(nameof(contextAccessor));
        }


        public int StartFrom {
            get {
                if (int.TryParse(contextAccessor.HttpContext?.Request.Query["sf"], out var sf)) {

                    return sf;
                }
                return 0;
            }
        }
        public int PageSize {
            get {
                if (int.TryParse(contextAccessor.HttpContext?.Request.Query["ps"], out var ps)) {

                    return ps;
                }
                return 25;
            }
        }
    }
}

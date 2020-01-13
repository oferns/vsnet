namespace VS.Core.Storage {
    using MediatR;
    using System;
    using VS.Abstractions.Storage.Paging;

    public class GetIndex : IRequest<PagedIndex> {

        public GetIndex(Uri prefix, int pageSize, string token) {
            Prefix = prefix;
            PageSize = pageSize;
            Token = token;
        }

        public Uri Prefix { get; }
        public int PageSize { get; }
        public string Token { get; }
    }
}

namespace VS.Core.Search {

    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    
    public class SearchRequestHandler : IRequestHandler<SearchRequest, SearchResponse[]> {
        
                
        public Task<SearchResponse[]> Handle(SearchRequest request, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
        }
    }
}

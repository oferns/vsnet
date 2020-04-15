namespace VS.Abstractions {

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface INavLinkProvider {

        ValueTask<IEnumerable<ContextualLink>> Links(string groupName, CancellationToken cancel);
    }
}
namespace VS.Abstractions {

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IActionProvider {

        ValueTask<IEnumerable<ContextualLink>> Actions(string formName, CancellationToken cancel);
    }
}

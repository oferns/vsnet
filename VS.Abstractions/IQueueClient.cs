namespace VS.Abstractions {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IQueueClient {

        Task<bool> Create(string queue, CancellationToken cancel);

        Task<bool> Destroy(string queue, CancellationToken cancel);


    }
}
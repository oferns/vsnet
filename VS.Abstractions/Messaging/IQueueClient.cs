namespace VS.Abstractions.Messaging {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IQueueClient {

        Task<Message<M>> Send<M>(string queue, Message<M> message, CancellationToken cancel) where M : class;


    }
}

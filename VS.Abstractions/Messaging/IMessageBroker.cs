namespace VS.Abstractions.Messaging {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;


    /// <summary>
    /// Generic client for Queue Brokerage
    /// </summary>
    /// <typeparam name="T">The type of Queue being created (ActiveMQQueue etc)</typeparam>
    public interface IMessageBroker<T> {

        Task<T> Create(string brokerName, CancellationToken cancel);

        Task<T> Destroy(string brokerId, CancellationToken cancel);

        Task<PagedBrokerIndex<T>> List(int pageSize, string token, CancellationToken cancel);
    }
}
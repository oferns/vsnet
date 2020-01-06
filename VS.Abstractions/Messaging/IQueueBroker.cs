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
    public interface IQueueBroker<T> {

        T Create(string queueName);

        T Destroy(string queueName);


        IEnumerable<T> List(bool forceRefresh);
    }
}
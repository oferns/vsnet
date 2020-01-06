namespace VS.Abstractions.Messaging {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITopicBroker<T> {

        T Create(string topicName);

        T Destroy(string topicName);


        IEnumerable<T> List(bool forceRefresh);
    }
}

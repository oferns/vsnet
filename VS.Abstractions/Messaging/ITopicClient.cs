namespace VS.Abstractions.Messaging {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITopicClient<T> {


        Message<M> Broadcast<M>(string topic, Message<M> message) where M : class;

    }
}

namespace VS.Abstractions.Messaging {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IQueueClient<T> {

        Message<M> Send<M>(string queue, Message<M> message) where M : class;

    }
}

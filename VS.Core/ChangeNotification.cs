namespace VS.Core {

    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ChangeNotification<T> : INotification {

        public ChangeNotification(IBaseRequest request, T result) {
            Request = request;
            Result = result;
        }

        public IBaseRequest Request { get; }
        public T Result { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace VS.Abstractions.Messaging {
    public interface ISubscribe {


        event EventHandler MessageRecieved;
    }
}

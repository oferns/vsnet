using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace VS.Core.Analytics {
    public class UserEvent : IRequest {


        
        public IEnumerable<string> Events { get;  set; }
    }
}

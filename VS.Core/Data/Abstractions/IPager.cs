
namespace VS.Core.Data.Abstractions {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IPager {

        int StartFrom { get; }
        int PageSize { get; }

    }
}

using System.Collections.Generic;

namespace VS.Abstractions {

    /// <summary>
    /// An interface for client messaging 
    /// </summary>
    public interface IFlashMessager {

        void Flash(string level, string message);

        IEnumerable<KeyValuePair<string, object>> GetMessages();
    }
}

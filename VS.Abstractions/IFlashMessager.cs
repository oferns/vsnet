
namespace VS.Abstractions {

    using System.Collections.Generic;

    /// <summary>
    /// An interface for client messaging 
    /// </summary>
    public interface IFlashMessager {

        void Flash(string level, string message);

        IEnumerable<KeyValuePair<string, string>> GetMessages();
    }
}

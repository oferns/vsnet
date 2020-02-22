
namespace VS.Mvc._Services {

    using System.Collections.Generic;
    using VS.Abstractions;

    public class HttpFlashMessager : IFlashMessager {

        private readonly List<KeyValuePair<string, string>> messages = new List<KeyValuePair<string, string>>();

        public void Flash(string level, string message) {
            messages.Add(KeyValuePair.Create(level, message));
        }

        public IEnumerable<KeyValuePair<string, string>> GetMessages() {
            return messages;
        }
    }
}

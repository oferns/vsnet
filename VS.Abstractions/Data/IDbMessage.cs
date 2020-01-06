namespace VS.Abstractions.Data {
    
    /// <summary>
    /// An interface to represent a message from the database to the client.
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>    
    public interface IDbMessage<T> where T : class {
        
        public T Message { get; }
    }
}

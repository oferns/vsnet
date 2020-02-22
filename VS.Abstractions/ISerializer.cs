namespace VS.Abstractions {

    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISerializer {

        ValueTask<Byte[]> SerializeToByteArray(Type type, object obj, CancellationToken cancel);
        ValueTask<Byte[]> SerializeToByteArray<T>(T obj, CancellationToken cancel);

        ValueTask<T> Deserialize<T>(ReadOnlyMemory<byte> buffer, CancellationToken cancel);
        ValueTask<T> Deserialize<T>(Stream stream, CancellationToken cancel);

        ValueTask<object> Deserialize(Type t, ReadOnlyMemory<byte> buffer, CancellationToken cancel);
        ValueTask<object> Deserialize(Type t, Stream stream, CancellationToken cancel);


        ValueTask SerializeToStream(Type type, object obj, Stream stream, CancellationToken cancel);
        ValueTask SerializeToStream<T>(T obj, Stream stream, CancellationToken cancel);

        ValueTask<string> SerializeToJson(Type type, object obj, CancellationToken cancel);
        ValueTask<string> SerializeToJson<T>(T obj, CancellationToken cancel);

        ValueTask<object> DeserializeFromJson(string json, CancellationToken cancel);
        ValueTask<T> DeserializeFromJson<T>(string json, CancellationToken cancel);

    }
}

namespace VS.MPack {
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MessagePack;
    using VS.Abstractions;

    public class MPSerializer : ISerializer {

        private readonly MessagePackSerializerOptions options;

        public MPSerializer(MessagePackSerializerOptions options) {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public ValueTask<T> Deserialize<T>(ReadOnlyMemory<byte> buffer, CancellationToken cancel) {
            return new ValueTask<T>(MessagePackSerializer.Deserialize<T>(buffer));
        }

        public ValueTask<T> Deserialize<T>(Stream stream, CancellationToken cancel) {
            return MessagePackSerializer.DeserializeAsync<T>(stream, options, cancel);
        }

        public ValueTask<object> Deserialize(Type t, ReadOnlyMemory<byte> buffer, CancellationToken cancel) {
            return new ValueTask<object>(MessagePackSerializer.Deserialize(t, buffer, options));
        }

        public ValueTask<object> Deserialize(Type t, Stream stream, CancellationToken cancel) {
            return new ValueTask<object>(MessagePackSerializer.DeserializeAsync(t, stream, options, cancel));
        }

        public ValueTask<object> DeserializeFromJson(string json, CancellationToken cancel) {
            throw new NotImplementedException();
        }

        public ValueTask<T> DeserializeFromJson<T>(string json, CancellationToken cancel) {
            var bytes = MessagePackSerializer.ConvertFromJson(json, options, cancel);
            return new ValueTask<T>(MessagePackSerializer.Deserialize<T>(bytes, options, cancel));
        }

        public ValueTask<byte[]> SerializeToByteArray(Type type, object obj, CancellationToken cancel) {
            return new ValueTask<byte[]>(MessagePackSerializer.Serialize(type, obj, options, cancel));
        }

        public ValueTask<byte[]> SerializeToByteArray<T>(T obj, CancellationToken cancel) {
            return new ValueTask<byte[]>(MessagePackSerializer.Serialize<T>(obj, options, cancel));
        }

        public ValueTask<string> SerializeToJson(Type type, object obj, CancellationToken cancel) {
            var bytes = MessagePackSerializer.Serialize(type, obj, options, cancel);
            return new ValueTask<string>(MessagePackSerializer.ConvertToJson(bytes, options, cancel));
        }

        public ValueTask<string> SerializeToJson<T>(T obj, CancellationToken cancel) {
            return new ValueTask<string>(MessagePackSerializer.SerializeToJson<T>(obj, options, cancel));
        }

        public ValueTask SerializeToStream(Type type, object obj, Stream stream, CancellationToken cancel) {
            return new ValueTask(MessagePackSerializer.SerializeAsync(type, stream, obj, options, cancel));
        }

        public ValueTask SerializeToStream<T>(T obj, Stream stream, CancellationToken cancel) {
            return new ValueTask(MessagePackSerializer.SerializeAsync<T>(stream, obj, options, cancel));
        }
    }
}

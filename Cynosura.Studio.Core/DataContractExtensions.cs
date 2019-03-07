using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core
{
    public static class DataContractExtensions
    {
        public static string SerializeDataContract<T>(this T obj)
        {
            var ser = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                ser.WriteObject(stream, obj);
                stream.Seek(0, 0);
                return Encoding.UTF8.GetString(stream.GetBuffer());
            }
        }

        public static T DeserializeDataContract<T>(this string json, params JsonConverter[] converters) where T : new()
        {
            var ser = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                var buffer = Encoding.UTF8.GetBytes(json);
                stream.Write(buffer, 0, buffer.Length);
                stream.Seek(0, 0);
                var result = (T) ser.ReadObject(stream);
                return result;
            }
        }
    }
}
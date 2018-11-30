using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core
{
    public static class XmlExnensions
    {
        public static string SerializeToXml<T>(this T obj)
        {
            var ser = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                ser.Serialize(stream, obj);
                stream.Seek(0, 0);
                return Encoding.UTF8.GetString(stream.GetBuffer());
            }
        }

        public static T DeserializeFromXml<T>(this string json, params JsonConverter[] converters) where T : new()
        {
            var ser = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                var buffer = Encoding.UTF8.GetBytes(json);
                stream.Write(buffer,0,buffer.Length);
                stream.Seek(0, 0);
                var result = (T) ser.Deserialize(stream);
                return result;
            }
        }
    }
}
using Newtonsoft.Json;
using Steam_Authenticator.Internal;
using System.Text;

namespace Steam_Authenticator.Model
{
    public class JsonStreamSerializer<T> : IStreamSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public Stream Serialize()
        {
            string json = JsonConvert.SerializeObject(this);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            MemoryStream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void Deserialize(Stream message)
        {
            byte[] bytes = new byte[message.Length];
            message.Read(bytes);
            string json = Encoding.UTF8.GetString(bytes);
            Value = JsonConvert.DeserializeObject<T>(json);
        }

        [JsonIgnore]
        public T Value { get; private set; }
    }
}

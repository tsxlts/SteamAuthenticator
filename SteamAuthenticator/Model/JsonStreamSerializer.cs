using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Model
{
    public class JsonStreamSerializer : IStreamSerializer
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
            var type = GetType();

            byte[] bytes = new byte[message.Length];
            message.Read(bytes);
            string json = Encoding.UTF8.GetString(bytes);
            var value = JsonConvert.DeserializeObject(json, type);

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                property.SetValue(this, property.GetValue(value), null);
            }
        }
    }
}

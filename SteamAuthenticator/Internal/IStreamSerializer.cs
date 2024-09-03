
namespace Steam_Authenticator.Internal
{
    public interface IStreamSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public Stream Serialize();

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void Deserialize(Stream message);
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Standand.Framework.MessageBroker.Concrete.Serializers
{
    public class MessageSerializer
    {
        public List<byte[]> Serialize<TEntity>(TEntity message)
        {
            return Serialize(new List<TEntity>() { message });
        }

        public List<byte[]> Serialize<TEntity>(List<TEntity> messages)
        {
            List<byte[]> result = new List<byte[]>();
            
            result = messages.Select(it =>
            {
                string message = JsonConvert.SerializeObject(it);
                return UTF8Encoding.UTF8.GetBytes(message);
            })
            .ToList();

            return result;
        }

        public List<TEntity> Deserialize<TEntity>(byte[] message)
        {
            return Deserialize<TEntity>(new List<byte[]>() { message });
        }

        public List<TEntity> Deserialize<TEntity>(List<byte[]> messages)
        {
            List<TEntity> result = null;

            result = messages.Select(it =>
            {
                string message = UTF8Encoding.UTF8.GetString(it);
                return JsonConvert.DeserializeObject<TEntity>(message);
            })
            .ToList();

            return result;
        }
    }
}

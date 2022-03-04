using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace jsonserver.Web.Extensions
{
    public static class SessionExtensions
    {
        public static T Get<T>(ISession session, string key)
        {
            var value =  session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }

        public static void Set<T>(ISession session, string key, T value)
        {
            session.SetString(key: key, value: JsonSerializer.Serialize(value));
        }
    }
}

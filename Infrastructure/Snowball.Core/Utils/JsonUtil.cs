using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Snowball.Core.Utils
{
    public static class JsonUtil
    {
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">被序列化的对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return Serialize(obj, false);
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">被序列化的对象</param>
        /// <param name="camelCase">是否使用小驼峰命名法</param>
        /// <returns></returns>
        public static string Serialize(object obj, bool camelCase)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            if (obj.GetType() == typeof(string))
            {
                return obj.ToString();
            }

            var settings = new JsonSerializerSettings();
            if (camelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="TResult">反序列化接收类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns></returns>
        public static TResult Deserialize<TResult>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }
            try
            {
                return JsonConvert.DeserializeObject<TResult>(json);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 将源对象转换成指定类型的对象
        /// </summary>
        /// <typeparam name="TResult">指定对象类型</typeparam>
        /// <param name="obj">源对象</param>
        /// <returns></returns>
        public static TResult ToObject<TResult>(object obj)
        {
            if (obj == null)
            {
                return default;
            }

            if (obj is TResult result)
            {
                return result;
            }

            if (obj is string)
            {
                return Deserialize<TResult>(obj + string.Empty);
            }

            string json = Serialize(obj, false);
            return Deserialize<TResult>(json);
        }

        /// <summary>
        /// 是否为JSON数组
        /// </summary>
        /// <param name="obj">源对象</param>
        /// <returns></returns>
        public static bool IsJsonArray(object obj)
        {
            if (obj == null)
            {
                return true;
            }

            var json = Serialize(obj, false);
            return json.StartsWith("[");
        }

    }
}

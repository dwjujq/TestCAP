using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Order.API.Utils
{
    public class JsonExtension
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings;

        internal static JsonSerializerSettings CustomSerializerSettings;

        static JsonExtension()
        {
            _jsonSerializerSettings = DefaultSerializerSettings;
        }

        internal static JsonSerializerSettings DefaultSerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings();

                // 设置如何将日期写入JSON文本。默认值为“IsoDateFormat”
                //settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                // 设置在序列化和反序列化期间如何处理DateTime时区。默认值为 “RoundtripKind”
                //settings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
                // 设置在序列化和反序列化期间如何处理默认值。默认值为“Include”
                //settings.DefaultValueHandling = DefaultValueHandling.Include;
                // 设置写入JSON文本时DateTime和DateTimeOffset值的格式，以及读取JSON文本时预期的日期格式。默认值为“ yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK ”。
                settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                // 设置在序列化和反序列化期间如何处理空值。默认值为“Include”
                //settings.NullValueHandling = NullValueHandling.Include;
                // 设置序列化程序在将.net对象序列化为JSON时使用的契约解析器
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                // 设置如何处理引用循环(例如，类引用自身)。默认值为“Error”。
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 是否格式化文本
                settings.Formatting = Formatting.Indented;
                //支持将Enum 由默认 Number类型 转换为String
                //settings.SerializerSettings.Converters.Add(new StringEnumConverter());
                //将long类型转为string
                //settings.SerializerSettings.Converters.Add(new NumberConverter(NumberConverterShip.Int64));

                return settings;
            }
        }

        public static T Deserialize<T>(string json, JsonSerializerSettings serializerSettings = null)
        {
            if (string.IsNullOrEmpty(json)) return default;

            if (serializerSettings == null) serializerSettings = _jsonSerializerSettings;

            //值类型和String类型
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(json, typeof(T));
            }

            return JsonConvert.DeserializeObject<T>(json, CustomSerializerSettings ?? serializerSettings);
        }

        public static object Deserialize(string json, Type type, JsonSerializerSettings serializerSettings = null)
        {
            if (string.IsNullOrEmpty(json)) return default;

            if (serializerSettings == null) serializerSettings = _jsonSerializerSettings;

            return JsonConvert.DeserializeObject(json, type, CustomSerializerSettings ?? serializerSettings);
        }

        public static string Serialize<T>(T obj, JsonSerializerSettings serializerSettings = null)
        {
            if (obj is null) return string.Empty;
            if (obj is string) return obj.ToString();
            if (serializerSettings == null) serializerSettings = _jsonSerializerSettings;
            return JsonConvert.SerializeObject(obj, CustomSerializerSettings ?? serializerSettings);
        }
    }
}

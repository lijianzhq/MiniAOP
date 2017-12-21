using System;
using System.Collections;
using System.Collections.Generic;

//第三方的命名空间
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniAOP.TestConsole
{
    public static class MyJson
    {
        /// <summary>
        /// 把对象序列化成JSON，可以为匿名对象
        /// var o = new
        /// {
        ///    a = 1,
        ///    b = "Hello, World!",
        ///    c = new[] { 1, 2, 3 },
        ///    d = new Dictionary<string, int> { { "x", 1 }, { "y", 2 } }
        /// };
        /// </summary>
        /// <param name="target">可以传入匿名对象</param>
        /// <returns></returns>
        public static String Serialize(Object target)
        {
            return JsonConvert.SerializeObject(target);
        }

        /// <summary>
        /// 根据json反序列化成指定的对象实例
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object Deserialize(String json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        /// <summary>
        /// 把json序列化成一个匿名类型的对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="anonymous">提供的匿名类，例如：var anonymousClass = new { a = 0, b = "text" };</param>
        /// <returns></returns>
        public static T DeserializeAnonymous<T>(String json, T anonymous)
        {
            return JsonConvert.DeserializeAnonymousType<T>(json, anonymous);
        }

        /// <summary>
        /// 把json反序列化成字典对象（名值对）
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> DeserializeToDic(String json)
        {
            Object jo = JsonConvert.DeserializeObject(json);
            return ParsetoDictionary(jo) as Dictionary<String, Object>;
        }

        /// <summary>
        /// 根据json反序列化成对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        #region "私有方法"

        /// <summary>
        /// 把转换出来的JObject复制到Dictionary中
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static Object ParsetoDictionary(Object o)
        {
            if (o == null) return null;
            if (o.GetType() == typeof(String))
            {
                //判断是否符合2010-09-02T10:00:00的格式
                String s = o.ToString();
                if (s.Length == 19 && s[10] == 'T' && s[4] == '-' && s[13] == ':')
                    o = System.Convert.ToDateTime(o);
            }
            else if (o is JObject)
            {
                JObject jo = o as JObject;
                Dictionary<String, Object> result = new Dictionary<String, Object>();
                foreach (KeyValuePair<String, JToken> entry in jo)
                    result[entry.Key] = ParsetoDictionary(entry.Value);
                o = result;
            }
            else if (o is IList)
            {
                ArrayList list = new ArrayList();
                list.AddRange((o as IList));
                int i = 0, l = list.Count;
                for (; i < l; i++)
                    list[i] = ParsetoDictionary(list[i]);
                o = list;
            }
            else if (typeof(JValue) == o.GetType())
            {
                JValue v = (JValue)o;
                o = ParsetoDictionary(v.Value);
            }
            else
            {
            }
            return o;
        }

        #endregion
    }
}

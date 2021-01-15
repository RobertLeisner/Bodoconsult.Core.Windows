using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Bodoconsult.Core.Windows.Test.Helpers
{
    /// <summary>
    /// Helper class to handle JSON in JobExecuter
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// Load a object from a JSON file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T LoadJsonFile<T>(string fileName)
        {
            T job;

            // Crypted JSON
            if (new FileInfo(fileName).Extension == ".cjson")
            {

                var json = File.ReadAllText(fileName, Encoding.UTF8);

                json = PasswordHandler.Decrypt2(json);

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                job = JsonConvert.DeserializeObject<T>(json, settings);

                return job;
            }


            using (var file = File.OpenText(fileName))
            {
                var serializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                job = (T)serializer.Deserialize(file, typeof(T));

            }

            return job;
        }


        /// <summary>
        /// Load a object from a JSON resource
        /// </summary>
        /// <param name="resourceName">Fully qualified resource name</param>
        /// <returns>object of type T or null</returns>
        public static T LoadJsonFromResource<T>(string resourceName)
        {
            T job;

            var ass = Assembly.GetExecutingAssembly();
            var str = ass.GetManifestResourceStream(resourceName);

            if (str == null) return default(T);

            using (var file = new StreamReader(str))
            {
                var serializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                job = (T)serializer.Deserialize(file, typeof(T));

            }

            return job;
        }


        /// <summary>
        /// Serialize a object to JSON
        /// </summary>
        /// <param name="value">object to convert</param>
        /// <typeparam name="T">type of object to convert</typeparam>
        /// <returns></returns>
        public static string SerializeObject<T>(T value)
        {
            var sb = new StringBuilder(256);
            var sw = new StringWriter(sb, CultureInfo.InvariantCulture);

            var jsonSerializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            };

            jsonSerializer.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                jsonWriter.IndentChar = '\t';
                jsonWriter.Indentation = 1;

                jsonSerializer.Serialize(jsonWriter, value, typeof(T));
            }

            return sw.ToString();
        }


        /// <summary>
        /// Save an object of type T as JSON to a file
        /// </summary>
        /// <param name="fileName">full path for the JSON file</param>
        /// <param name="data">object to serialize to JSON</param>
        /// <typeparam name="T">type of the object to serialize to JSON</typeparam>
        /// <returns>JSON string</returns>
        public static string SaveAsFile<T>(string fileName, T data)
        {

            var json = SerializeObject(data);

            //using (var file = File.CreateText(fileName))
            //{
            //    var serializer = new JsonSerializer
            //    {
            //        Formatting = Formatting.Indented,
            //        TypeNameHandling = TypeNameHandling.Objects,
            //        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            //    };
            //    serializer.Serialize(file, data);
            //}


            if (new FileInfo(fileName).Extension == ".cjson")
            {
                json = PasswordHandler.Encrypt2(json);
            }

            File.WriteAllText(fileName, json, Encoding.UTF8);

            return json;
        }
    }
}
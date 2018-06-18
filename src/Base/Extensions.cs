namespace BGuidinger.Base
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Text;

    public static partial class Extensions
    {
        public static TObject FromJson<TObject>(this string input)
        {
            if (string.IsNullOrEmpty(input)) return default;

            var settings = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true
            };

            var serializer = new DataContractJsonSerializer(typeof(TObject), settings);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                return (TObject)serializer.ReadObject(stream);
            }
        }
        public static string ToJson<TObject>(this TObject input)
        {
            var settings = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true
            };

            var serializer = new DataContractJsonSerializer(typeof(TObject), settings);
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                serializer.WriteObject(stream, input);
                stream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static TResult GetValue<TResult>(this Dictionary<string, dynamic> config, string key)
        {
            if (config.ContainsKey(key))
            {
                return (TResult)config[key];
            }
            else
            {
                return default;
            }
        }

        public static void WriteBody(this WebRequest request, Dictionary<string, string> parameters)
        {
            var body = Encoding.UTF8.GetBytes(parameters.UrlEncode());

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = body.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
            }
        }

        public static string UrlEncode(this Dictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value)}"));
        }

        public static Entity RetrieveSingle(this IOrganizationService service, QueryExpression query)
        {
            if (query == null)
            {
                throw new System.ArgumentNullException(nameof(query));
            }

            var results = service.RetrieveMultiple(query);
            if (results != null && results.Entities != null && results.Entities.Count > 0)
            {
                if (results.Entities.Count == 1)
                {
                    return results.Entities[0];
                }
                else
                {
                    throw new Exception("More than one entity was returned.");
                }
            }
            return null;
        }
    }
}
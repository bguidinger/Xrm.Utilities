using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;

namespace BGuidinger.Base
{
    public class TokenService : ITokenService
    {
        public Token GetToken(string resource, string username, string password)
        {
            var parameters = new Dictionary<string, string>()
            {
                ["grant_type"] = "password",
                ["client_id"] = "2ad88395-b77d-4561-9441-d0e40824f9bc",
                ["resource"] = resource,
                ["username"] = username,
                ["password"] = password,
            };
            var url = "https://login.microsoftonline.com/common/oauth2/token";
            var request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.WriteBody(parameters);

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(Token));
                return serializer.ReadObject(stream) as Token;
            }
        }
    }
}

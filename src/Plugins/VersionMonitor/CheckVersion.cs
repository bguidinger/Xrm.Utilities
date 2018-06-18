namespace BGuidinger.Utilities.VersionMonitor
{
    using Base;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    public class CheckVersion : Plugin
    {
        public CheckVersion(string unsecure, string secure) : base(unsecure, secure) { }

        public override void OnExecute(IPluginProvider provider)
        {
            // Get configuration values
            var resource = SecureConfig.GetValue<string>("resource");
            var username = SecureConfig.GetValue<string>("username");
            var password = SecureConfig.GetValue<string>("password");

            // Get access token
            var token = provider.TokenService.GetToken(resource, username, password);

            // Get current versions
            var current = GetCurrentVersions(token);
            var previous = GetPreviousVersions(provider.OrganizationAdminService);

            var isUpdated = current.Application > previous.Application || current.Database > previous.Database;
            if (isUpdated)
            {
                var entity = new Entity("bg_versionlog")
                {
                    ["bg_application"] = current.Application.ToString(),
                    ["bg_database"] = current.Database.ToString()
                };
                provider.OrganizationAdminService.Create(entity);
            }

            var output = provider.ExecutionContext.OutputParameters;
            output["Application"] = current.Application.ToString();
            output["Database"] = current.Database.ToString();
            output["IsUpdated"] = isUpdated;
        }

        private Versions GetCurrentVersions(Token token)
        {
            return new Versions()
            {
                Application = GetApplicationVersion(token),
                Database = GetDatabaseVersion(token)
            };
        }
        private Version GetApplicationVersion(Token token)
        {
            var url = "/main.aspx";
            var regex = new Regex("APPLICATION_FULL_VERSION = '(.*?)'");

            var response = GetResponse(url, token, regex);

            return Version.Parse(response);
        }
        private Version GetDatabaseVersion(Token token)
        {
            var url = "/api/data/v9.0/RetrieveVersion()";
            var regex = new Regex("\"Version\":\"(.*?)\"");

            var response = GetResponse(url, token, regex);

            return Version.Parse(response);
        }

        private Versions GetPreviousVersions(IOrganizationService service)
        {
            var query = new QueryExpression("bg_versionlog");
            query.AddOrder("createdon", OrderType.Descending);
            query.TopCount = 1;
            query.ColumnSet = new ColumnSet("bg_application", "bg_database");
            var versionLog = service.RetrieveSingle(query);
            var application = versionLog?.GetAttributeValue<string>("bg_application") ?? "0.0.0.0";
            var database = versionLog?.GetAttributeValue<string>("bg_database") ?? "0.0.0.0";

            return new Versions()
            {
                Application = Version.Parse(application),
                Database = Version.Parse(database)
            };
        }

        private string GetResponse(string url, Token token, Regex regex)
        {
            var request = WebRequest.CreateHttp(token.Resource + url);
            request.Method = "GET";
            request.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var data = reader.ReadToEnd();

                var matches = regex.Match(data);
                return matches.Groups?[1]?.Value;
            }
        }
    }
}
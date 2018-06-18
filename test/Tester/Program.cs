namespace BGuidinger.Tests
{
    using Base;
    using Utilities.VersionMonitor;
    using Microsoft.Xrm.Tooling.Connector;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            var secure = new Dictionary<string, dynamic>()
            {
                ["resource"] = "https://organization.crm.dynamics.com",
                ["username"] = "admin@organization.com",
                ["password"] = "Pass@word!",
            };

            var connectionString = $"AuthType=Office365; Username={secure["username"]}; Password={secure["password"]}; Url={secure["resource"]};";
            var service = new CrmServiceClient(connectionString);

            var context = new FakePluginExecutionContext();

            var provider = new FakePluginProvider
            {
                ExecutionContext = context,
                OrganizationService = service,
                OrganizationAdminService = service
            };

            var request = new CheckVersion(null, secure.ToJson());
            request.OnExecute(provider);
        }
    }
}
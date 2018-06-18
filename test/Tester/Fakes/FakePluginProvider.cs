namespace BGuidinger.Tests
{
    using Base;
    using Microsoft.Xrm.Sdk;

    public class FakePluginProvider : IPluginProvider
    {
        public IPluginExecutionContext ExecutionContext { get; set; }

        public IOrganizationService OrganizationService { get; set; }
        public IOrganizationService OrganizationAdminService { get; set; }

        public ILoggingService LoggingService { get; set; }
        public Base.ITokenService TokenService { get; set; } = new TokenService();

        public EntityReference Target { get; set; }
    }
}

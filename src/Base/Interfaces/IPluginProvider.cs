namespace BGuidinger.Base
{
    using Microsoft.Xrm.Sdk;

    public interface IPluginProvider
    {
        IPluginExecutionContext ExecutionContext { get; }
        IOrganizationService OrganizationService { get; }
        IOrganizationService OrganizationAdminService { get; }
        ILoggingService LoggingService { get; }
        ITokenService TokenService { get; }

        EntityReference Target { get; }
    }
}
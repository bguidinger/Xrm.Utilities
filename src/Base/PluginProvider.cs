namespace BGuidinger.Base
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Extensions;
    using System;

    public class PluginProvider : IPluginProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public PluginProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        private IPluginExecutionContext _executionContext;
        public IPluginExecutionContext ExecutionContext
        {
            get
            {
                if (_executionContext == null)
                {
                    _executionContext = _serviceProvider.Get<IPluginExecutionContext>();
                }

                return _executionContext;
            }
        }

        private IOrganizationService _organizationService;
        public IOrganizationService OrganizationService
        {
            get
            {
                if (_organizationService == null)
                {
                    _organizationService = _serviceProvider.GetOrganizationService(ExecutionContext.UserId);
                }

                return _organizationService;
            }
        }

        private IOrganizationService _organizationAdminService;
        public IOrganizationService OrganizationAdminService
        {
            get
            {
                if (_organizationAdminService == null)
                {
                    var factory = _serviceProvider.Get<IOrganizationServiceFactory>();
                    _organizationAdminService = factory.CreateOrganizationService(null);
                }

                return _organizationAdminService;
            }
        }

        private ILoggingService _loggingService;
        public ILoggingService LoggingService
        {
            get
            {
                if (_loggingService == null)
                {
                    var tracingService = _serviceProvider.Get<ITracingService>();
                    _loggingService = new TraceLoggingService(tracingService);
                }

                return _loggingService;
            }
        }

        private ITokenService _tokenService;
        public ITokenService TokenService
        {
            get
            {
                if (_tokenService == null)
                {
                    _tokenService = new TokenService();
                }

                return _tokenService;
            }
        }

        private EntityReference _target;
        public EntityReference Target
        {
            get
            {
                if (_target == null)
                {
                    _target = ExecutionContext.InputParameterOrDefault<EntityReference>("Target");
                }

                return _target;
            }
        }
    }
}
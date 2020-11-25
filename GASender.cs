using Ninject;
using NLog;
using System;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Wallet.GASender.Api.Impl;
using Wallet.GASender.Config;
using Wallet.GASender.Container;
using Wallet.GASender.Handlers;
using Wallet.GASender.Logs;
using Wallet.GASender.Repository;
using Wallet.Messaging;
using Wallet.Messaging.Impl;
using Wallet.Messaging.LocalStorage;

namespace Wallet.GASender
{
    public partial class GASender : ServiceBase
    {
        private readonly Logger _logger = LogManager.GetLogger("GASender");
        private readonly IEasyNetQBus _messageBus;
        private readonly Repository.IRepository _repository;
        private IDisposable _consumer;
        private HttpSelfHostServer _server;
        private readonly PaymentsService _sendService;

        public GASender()
        {
            InitializeComponent();

            _logger = LogManager.GetLogger(GetType().Name);
            try
            {
                var busConfiguration = ConfigService.Instance.Bus;

                if (busConfiguration == null)
                    throw new Exception("BusConfiguration must be set in config");

                var kernel = new StandardKernel();

                kernel.Bind<IMessageBodySerializer>().To<DefaultJsonSerializer>().InSingletonScope();
                kernel.Bind<IBusLogger>().To<CustomNLogger>().InSingletonScope();

                NinjectContainerAdapter.CreateInstance(kernel);
                BusFactory.SetContainerFactory(NinjectContainerAdapter.GetInstance);
                _messageBus = BusFactory.CreateBus();

                _repository = new Repository.Repository(ConfigService.Instance.Payments.W1ConnectionString);

                _sendService = new PaymentsService(
                    _repository,
                    ConfigService.Instance.Common.GoogleAnaliticsUrl,
                    ConfigService.Instance.Common.TrackingId,
                    ConfigService.Instance.Payments.Interval,
                    ConfigService.Instance.Common.GoogleAnaliticsTimeout,
                    ConfigService.Instance.Payments.StartTime,
                    ConfigService.Instance);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _logger.Info("Try start");

                var ninjectResolver = new NinjectResolver();
                ninjectResolver.Kernel.Bind<IMonitorService>().To<MonitorService>()
                    .WithConstructorArgument("messageBus", _messageBus)
                    .WithConstructorArgument("dateFromUpdater", ConfigService.Instance)
                    .WithConstructorArgument("repository", _repository);

                var config = new HttpSelfHostConfiguration(ConfigService.Instance.HealthCheck.Url);
                config.DependencyResolver = ninjectResolver;
                config.Routes.MapHttpRoute(name: "API", routeTemplate: "{controller}/{action}");

                _server = new HttpSelfHostServer(config);
                _server.OpenAsync().Wait();

                _logger.Info("Api server started");

                _messageBus.AddConsumerHandler(new GAActivationMessageHandler());
                _messageBus.AddConsumerHandler(new GAConnectionMessageHandler());

                _consumer = _messageBus.Consume("main");

                _sendService.Start();

                _logger.Info("Start successful");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        protected override void OnStop()
        {
            _logger.Info("Try stop");

            if (_server != null)
                _server.CloseAsync().Wait();

            _logger.Info("Api server stop successful");

            _consumer?.Dispose();
            _messageBus.Dispose();

            _sendService.Dispose();

            _logger.Info("Stop successful");

            _logger.Factory.Flush(5000);

            if (LogManager.Configuration != null)
            {
                var allTargets = LogManager.Configuration.AllTargets;
                foreach (var target in allTargets)
                    target.Dispose();
            }
        }
    }
}

using System;
using System.Configuration;
using Wallet.Messaging.Config;

namespace Wallet.GASender.Config
{
    public class ConfigService : IDateFromUpdater
    {
        private readonly Configuration _config;

        private static ConfigService _instance;

        private ConfigService()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static ConfigService Instance => _instance ?? (_instance = new ConfigService());

        public BusConfiguration Bus => _config.GetSection("BusConfiguration") as BusConfiguration;

        public HealthCheckSection HealthCheck => _config.GetSection("healthCheck") as HealthCheckSection;

        public CommonSection Common => _config.GetSection("common") as CommonSection;

        public PaymentsSection Payments => _config.GetSection("payments") as PaymentsSection;

        public DateTime DateFrom => Payments.DateFrom;

        public void SetPaymentsDateFrom(DateTime date)
        {
            Payments.DateFrom = date;
            _config.Save(ConfigurationSaveMode.Full);
        }
    }
}

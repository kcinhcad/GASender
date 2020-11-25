using System;
using Wallet.GASender.Config;
using Wallet.GASender.Repository;
using Wallet.Messaging;

namespace Wallet.GASender.Api.Impl
{
    internal class MonitorService : IMonitorService
    {
        private readonly IEasyNetQBus _messageBus;
        private readonly IDateFromUpdater _dateFromUpdater;
        private readonly IRepository _repository;

        public MonitorService(IEasyNetQBus messageBus, IDateFromUpdater dateFromUpdater, IRepository repository)
        {
            _messageBus = messageBus;
            _dateFromUpdater = dateFromUpdater;
            _repository = repository;
        }

        public bool IsBusAvailable()
        {
            return _messageBus.IsConnected;
        }

        public bool IsDatabaseAvailable()
        {
            return _repository.IsConnected;
        }

        public bool IsPaymentServiceAvailable()
        {
            return _dateFromUpdater.DateFrom != DateTime.MinValue && (DateTime.Today - _dateFromUpdater.DateFrom.Date).TotalDays <= 1;
        }
    }
}

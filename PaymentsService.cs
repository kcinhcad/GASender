using NLog;
using System;
using System.Threading;
using Wallet.GASender.Client;
using Wallet.GASender.Config;
using Wallet.GASender.Repository;

namespace Wallet.GASender
{
    public class PaymentsService : IDisposable
    {
        private readonly Logger _logger;
		private readonly IRepository _repository;
		private readonly TimeSpan _interval;
        private readonly CancellationTokenSource _tokenSource;
		private readonly GoogleAnaliticsClient _client;
		private readonly IDateFromUpdater _dateFromUpdater;
		private readonly TimeSpan _startTime;

		public PaymentsService(IRepository repository, string gaUrl, string trackingId, int interval, int timeout, TimeSpan startTime, IDateFromUpdater dateFromUpdater)
		{
			_logger = LogManager.GetLogger(GetType().Name);
			_repository = repository;
			_interval = new TimeSpan(0, 0, 0, interval);
			_tokenSource = new CancellationTokenSource();
			_client = new GoogleAnaliticsClient(gaUrl, trackingId, timeout);
			_startTime = startTime;
			_dateFromUpdater = dateFromUpdater;
		}

		public void Start()
		{
			ThreadPool.QueueUserWorkItem(x => Send(_tokenSource.Token));
		}

		private async void Send(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				var dateFrom = _dateFromUpdater.DateFrom;
				if (dateFrom == DateTime.MinValue)
					dateFrom = DateTime.UtcNow.Date.AddDays(-1);

				var needWait = true;

				var daysDiff = (DateTime.Today - dateFrom).TotalDays;
				if (daysDiff > 0 && (daysDiff > 1 || _startTime <= DateTime.Now.TimeOfDay))
				{
					try
					{
						while (dateFrom < DateTime.Today)
						{
							_logger.Info("Start send operations for " + dateFrom.ToString("yyyy-MM-dd HH:mm:ss"));

							var operations = _repository.GetInvoiceOperationsForGA(dateFrom);

							_logger.Info("Operations count: " + operations.Count);

							for (int i = 0; i < operations.Count; i++)
							{
								if (await _client.Payments(operations[i], i == operations.Count - 1))
									_dateFromUpdater.SetPaymentsDateFrom(operations[i].UpdateDate);
							}
							dateFrom = dateFrom.Date.AddDays(1);
							_dateFromUpdater.SetPaymentsDateFrom(dateFrom);

							_logger.Info("End send operations");
						}
					}
					catch (OperationCanceledException)
					{
						_logger.Info("Operation canceled");
						return;
					}
					catch (Exception exception)
					{
						_logger.Error(exception);
					}
				}

				if (needWait)
				{
					Thread.Sleep(_interval);
				}
			}
		}

		public void Dispose()
		{
			_tokenSource.Cancel();
		}
	}
}

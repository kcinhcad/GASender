using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Wallet.GASender.Repository;

namespace Wallet.GASender.Client
{
    public class GoogleAnaliticsClient
    {
        private const int BatchSize = 20;

        private readonly Logger _logger;
        private readonly string _url;
        private readonly string _trackingId;
        private readonly int _timeout;

        private readonly List<KeyValuePair<string, string>[]> _batchPayments;

        public GoogleAnaliticsClient(string url, string trackingId, int timeout)
        {
            _logger = LogManager.GetLogger(GetType().Name);
            _url = url;
            _trackingId = trackingId;
            _batchPayments = new List<KeyValuePair<string, string>[]>();
            _timeout = timeout;
        }

        public async Task Activation(long userId, string userTitle)
        {
            await collect(userId.ToString(), "event", new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
            });
        }

        public async Task Connection(long userId, string userTitle)
        {
            await collect(userId.ToString(), "event", new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
            });
        }

        public async Task<bool> Payments(Payment payment, bool lastItem)
        {
            bool sent = false;
            if (payment.IsFirst)
            {
                _batchPayments.Add(new KeyValuePair<string, string>[] {
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                    });
                if (_batchPayments.Count >= BatchSize)
                {
                    await batch(_batchPayments);
                    _batchPayments.Clear();
                }
            }

            _batchPayments.Add(new KeyValuePair<string, string>[] {
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                    });
            if (_batchPayments.Count >= BatchSize)
            {
                await batch(_batchPayments);
                _batchPayments.Clear();
                sent = true;
            }

            if (payment.FeeFromMerchant > 0 || payment.FeeFromPayer > 0 || payment.FeeToMerchant > 0 || payment.FeeToAggregator > 0)
            {
                _batchPayments.Add(new KeyValuePair<string, string>[] {
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                    });
                if (_batchPayments.Count >= BatchSize)
                {
                    await batch(_batchPayments);
                    _batchPayments.Clear();
                    sent = true;
                }
            }

            if (payment.FeeToAgent > 0 || payment.FeeFomAgent > 0 || payment.FeeToIpsp > 0)
            {
                _batchPayments.Add(new KeyValuePair<string, string>[] {
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                        new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"),
                    });
                if (_batchPayments.Count >= BatchSize)
                {
                    await batch(_batchPayments);
                    _batchPayments.Clear();
                    sent = true;
                }
            }
            if (_batchPayments.Count > 0 && lastItem)
            {
                await batch(_batchPayments);
                _batchPayments.Clear();
                sent = true;
            }
            return sent;
        }

        private async Task collect(string clientId, string hitType, KeyValuePair<string, string>[] additionalParams)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, _timeout);

                var formParams = new List<KeyValuePair<string, string>>();
                formParams.Add(new KeyValuePair<string, string>("*** sensitive ***", "*** sensitive ***"));
                formParams.AddRange(additionalParams);
                using (var content = new FormUrlEncodedContent(formParams))
                {
                    var requestBody = await content.ReadAsStringAsync();
                    using (var response = await client.PostAsync(_url + "/collect", content))
                    {
                        _logger.Info($"Request: {_url}/collect{Environment.NewLine}Content: {requestBody}{Environment.NewLine}Response: {response.StatusCode}");

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception("Response status not success");
                        }
                    }
                }
            }
        }

        private async Task batch(List<KeyValuePair<string, string>[]> ps)
        {
            if (ps.Count > BatchSize)
                throw new Exception("Error payments count in batch");

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, _timeout);

                var b = new StringBuilder();

                foreach (var p in ps)
                    b.AppendLine(await new FormUrlEncodedContent(p).ReadAsStringAsync());
                
                var content = b.ToString();
                using (var response = await client.PostAsync(_url + "/batch", new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")))
                {
                    _logger.Info($"Request: {_url}/batch{Environment.NewLine}Content: {content}{Environment.NewLine}Response: {response.StatusCode}");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Response status not success. Status code: " + response.StatusCode);
                    }
                }
            }
        }
    }
}

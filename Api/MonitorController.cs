using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Wallet.GASender.Api.Impl;

namespace Wallet.GASender.Api
{
    public class MonitorController : ApiController
    {
        private readonly Logger _logger = LogManager.GetLogger("MonitorController");
        private readonly IMonitorService _monitorService;

        public MonitorController(IMonitorService monitorService)
        {
            _monitorService = monitorService;
        }

        [HttpGet]
        public HttpResponseMessage HealthCheck()
        {
            try
            {
                HttpStatusCode status = HttpStatusCode.OK;
                string message = null;

                if (!_monitorService.IsBusAvailable())
                {
                    status = HttpStatusCode.ServiceUnavailable;
                    message = "Недоступна шина";
                }
                if (!_monitorService.IsDatabaseAvailable())
                {
                    status = HttpStatusCode.ServiceUnavailable;
                    message = "Недоступна бд";
                }
                if (!_monitorService.IsPaymentServiceAvailable())
                {
                    status = HttpStatusCode.ServiceUnavailable;
                    message = "Не отправлены метрики по платежам";
                }

                HttpResponseMessage response = new HttpResponseMessage(status);

                if (!string.IsNullOrEmpty(message))
                    response.Content = new ObjectContent(typeof(ResponseMessage),
                        new ResponseMessage(message),
                        new JsonMediaTypeFormatter());
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}
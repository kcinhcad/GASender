using System;
using System.Threading.Tasks;
using NLog;
using Wallet.BusMessages;
using Wallet.GASender.Client;
using Wallet.Messaging;

namespace Wallet.GASender.Handlers
{
    public class GAActivationMessageHandler : IHandler<GAActivationMessage>
    {
        private readonly Logger _logger;
        private readonly GoogleAnaliticsClient _client;

        public GAActivationMessageHandler()
        {
            _logger = LogManager.GetLogger(GetType().Name);
            _client = new GoogleAnaliticsClient(
                Config.ConfigService.Instance.Common.GoogleAnaliticsUrl,
                Config.ConfigService.Instance.Common.TrackingId,
                Config.ConfigService.Instance.Common.GoogleAnaliticsTimeout);
        }

        public async Task ExecuteAsync(GAActivationMessage message, Guid messageId)
        {
            _logger.Info($"Start processig message {messageId}. UserId: {message.UserId}");

            try
            {
                await _client.Activation(message.UserId, message.UserTitle);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error processig message {messageId}. UserId: {message.UserId}");
                throw;
            }

            _logger.Info($"End processig message {messageId}. UserId: {message.UserId}");
        }
    }
}

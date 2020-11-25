using System;
using Wallet.Messaging;

namespace Wallet.GASender.Logs
{
    public class CustomNLogger : IBusLogger
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetLogger("MessageBusLog");

        #region Implementation of IMessageBusLogger

        public void Trace(string format, params object[] args)
        {
            _logger.Trace(format, args);
        }

        public void Debug(string format, params object[] args)
        {
            _logger.Debug(format, args);
        }

        public void Info(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            _logger.Warn(format, args);
        }

        public void Error(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception, new object[] { });
        }

        public void Fatal(string format, params object[] args)
        {
            _logger.Fatal(format, args);
        }

        #endregion
    }
}

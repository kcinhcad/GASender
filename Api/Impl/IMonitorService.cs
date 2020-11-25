namespace Wallet.GASender.Api.Impl
{
    public interface IMonitorService
    {
        /// <summary>
        /// Проверить доступность шины
        /// </summary>
        bool IsBusAvailable();

        /// <summary>
        /// Проверить доступность базы данных W1
        /// </summary>
        bool IsDatabaseAvailable();

        /// <summary>
        /// Проверить работоспособность обработки платежных операций
        /// </summary>
        bool IsPaymentServiceAvailable();
    }
}

using System;
using System.Collections.Generic;

namespace Wallet.GASender.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// Проверить подключение к бд
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Получить список платежных операций за указанную дату
        /// </summary>
        /// <returns></returns>
        List<Payment> GetInvoiceOperationsForGA(DateTime date);
    }
}

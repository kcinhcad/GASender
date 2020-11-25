using System;

namespace Wallet.GASender.Config
{
    public interface IDateFromUpdater
    {
        DateTime DateFrom { get; }

        void SetPaymentsDateFrom(DateTime date);
    }
}

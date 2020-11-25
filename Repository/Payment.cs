using System;

namespace Wallet.GASender.Repository
{
    public struct Payment
    {
        public long UserId { get; }
        public string UserTitle { get; }

        public long OperationId { get; }
        public bool IsFirst { get; }
        public decimal Amount { get; }

        public decimal FeeFromMerchant { get; }
        public decimal FeeFromPayer { get; }
        public decimal FeeToMerchant { get; }
        public decimal FeeToAggregator { get; }

        public decimal FeeToAgent { get; }
        public decimal FeeFomAgent { get; }
        public decimal FeeToIpsp { get; }

        public DateTime UpdateDate { get; }

        public Payment(long userId, string userTitle, long operationId, bool isFirst, decimal amount,
            decimal feeFromMerchant, decimal feeFromPayer, decimal feeToMerchant, decimal feeToAggregator,
            decimal feeToAgent, decimal feeFomAgent, decimal feeToIpsp, DateTime updateDate)
        {
            UserId = userId;
            UserTitle = userTitle;

            OperationId = operationId;
            IsFirst = isFirst;
            Amount = amount;

            FeeFromMerchant = feeFromMerchant;
            FeeFromPayer = feeFromPayer;
            FeeToMerchant = feeToMerchant;
            FeeToAggregator = feeToAggregator;

            FeeToAgent = feeToAgent;
            FeeFomAgent = feeFomAgent;
            FeeToIpsp = feeToIpsp;

            UpdateDate = updateDate;
        }
    }
}

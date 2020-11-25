namespace Wallet.GASender.Api.Impl
{
    public class ResponseMessage
    {
        public string Message { get; }

        public ResponseMessage(string message)
        {
            Message = message;
        }
    }
}

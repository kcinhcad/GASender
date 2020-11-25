using System.ServiceProcess;

namespace Wallet.GASender
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new GASender()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

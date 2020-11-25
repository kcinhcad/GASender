using System;
using EasyNetQ;
using Ninject;
using Wallet.Messaging;

namespace Wallet.GASender.Container
{
    public class NinjectContainerAdapter : IBusContainer, IDisposable
    {
        private static NinjectContainerAdapter _instance;
        private static readonly object LockObject = new object();

        public static void CreateInstance(IKernel ninjectContainer)
        {
            if (_instance != null)
                return;

            _instance = new NinjectContainerAdapter(ninjectContainer);
        }

        public static NinjectContainerAdapter GetInstance()
        {
            if (_instance == null)
            {
                lock (LockObject)
                {
                    if (_instance == null)
                        throw new NullReferenceException("NinjectContainerAdapter.Instance is null.");
                }
            }
            return _instance;
        }

        private readonly IKernel _ninjectContainer;

        private NinjectContainerAdapter(IKernel ninjectContainer)
        {
            _ninjectContainer = ninjectContainer;
        }

        public TService Resolve<TService>() where TService : class
        {
            return _ninjectContainer.Get<TService>();
        }

        public IServiceRegister Register<TService>(Func<EasyNetQ.IServiceProvider, TService> serviceCreator)
            where TService : class
        {
            if (!IsAlreadyRegistered<TService>())
                _ninjectContainer.Bind<TService>().ToMethod(ctx => serviceCreator(this)).InSingletonScope();

            return this;
        }

        public IServiceRegister Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            if (!IsAlreadyRegistered<TService>())
                _ninjectContainer.Bind<TService>().ToMethod(ctx => ctx.Kernel.Get<TImplementation>()).InSingletonScope();

            return this;
        }

        private bool IsAlreadyRegistered<TService>() where TService : class
        {
            return _ninjectContainer.CanResolve<TService>();
        }

        public void Dispose()
        {
            _ninjectContainer.Dispose();
        }

        public string GetRegistrationList()
        {
            return string.Empty;
        }

        public void DropInstanceByServiceType(Type serviceType)
        {
            var t = typeof(IKernel)
                .GetMethod("Get", new Type[0])
                .MakeGenericMethod(serviceType)
                .Invoke(this, new object[0]);

            _ninjectContainer.Release(t);
        }

        public void DropAllInstances()
        {

        }
    }
}

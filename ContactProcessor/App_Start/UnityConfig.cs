using System;
using ContactProcessor.Controllers;
using ContactProcessor.Utilities.ConfigManager;
using ContactProcessor.Utilities.ContactFileReader;
using ContactProcessor.Utilities.ContactFileWriter;
using ContactProcessor.Utilities.EmailClient;
using ContactProcessor.Utilities.FileSystemHelper;
using ContactProcessor.Utilities.Logger;
using ContactProcessor.Utilities.Notifier;
using Microsoft.Practices.Unity;

namespace ContactProcessor.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<IConfigManager, ConfigManager>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmailClient, EmailClient>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogger, Logger>(new HierarchicalLifetimeManager());
            container.RegisterType<IContactFileReader, ContactFileReader>(new HierarchicalLifetimeManager());

            // let the factory classes be singleton as they don't change, but can be shared.
            container.RegisterType<IContactFileReaderFactory, ContactFileReaderFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IContactFileWriterFactory, ContactFileWriterFactory>(new ContainerControlledLifetimeManager());

            
            container.RegisterType<INotifier, NotifierEmail>(Utilities.Constants.IOC_NotifierEmail, new HierarchicalLifetimeManager());
            container.RegisterType<INotifier, NotifierText>(Utilities.Constants.IOC_NotifierText, new HierarchicalLifetimeManager());

            container.RegisterType<IFileSystemHelper, FileSystemHelper>(new ContainerControlledLifetimeManager());

            container.RegisterType<CSVReaderWriterController>(
                new InjectionConstructor(
                    new InjectionParameter(container.Resolve<IConfigManager>()),
                    new InjectionParameter(container.Resolve<ILogger>()),
                    new InjectionParameter(container.Resolve<INotifier>(Utilities.Constants.IOC_NotifierEmail)),
                    new InjectionParameter(container.Resolve<INotifier>(Utilities.Constants.IOC_NotifierText)),
                    new InjectionParameter(container.Resolve<IContactFileReaderFactory>()),
                    new InjectionParameter(container.Resolve<IContactFileWriterFactory>()),
                    new InjectionParameter(container.Resolve<IFileSystemHelper>())
                    )
                );

        }
    }
}

using ContactProcessor.Controllers;
using ContactProcessor.Models;
using ContactProcessor.Utilities.ConfigManager;
using ContactProcessor.Utilities.ContactFileReader;
using ContactProcessor.Utilities.ContactFileWriter;
using ContactProcessor.Utilities.EmailClient;
using ContactProcessor.Utilities.FileSystemHelper;
using ContactProcessor.Utilities.Logger;
using ContactProcessor.Utilities.Notifier;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ContactProcessor.UnitTests.Controllers
{
    [TestFixture]
    public class CSVReaderWriterControllerTests
    {
        private Mock<ILogger> mockLogger = new Mock<ILogger>();
        private Mock<IConfigManager> mockConfigManager = new Mock<IConfigManager>();

        private Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();

        private Mock<INotifier> mockNotifierEmail = new Mock<INotifier>();
        private Mock<INotifier> mockNotifierText = new Mock<INotifier>();

        private Mock<IContactFileReaderFactory> mockContactFileReaderFactory = new Mock<IContactFileReaderFactory>();
        private Mock<IContactFileWriterFactory> mockContactFileWriterFactory = new Mock<IContactFileWriterFactory>();

        private Mock<IContactFileReader> mockContactFileReader = new Mock<IContactFileReader>();
        private Mock<IContactFileWriter> mockContactFileWriter = new Mock<IContactFileWriter>();

        private Mock<IFileSystemHelper> mockFileSystemHelper = new Mock<IFileSystemHelper>();

        public CSVReaderWriterControllerTests()
        {
            //mockNotifierEmail = new NotifierEmail(mockConfigManager.Object, mockEmailClient.Object, mockLogger.Object);
            //mockNotifierText = new NotifierText();// mockConfigManager.Object, mockEmailClient.Object, mockLogger.Object);

            mockLogger.Setup(a => a.Log(It.IsAny<string>(), It.IsAny<LogLevel>())).Verifiable();

            mockContactFileReaderFactory.Setup(a => a.Create()).Returns(mockContactFileReader.Object);
            mockContactFileWriterFactory.Setup(a => a.Create()).Returns(mockContactFileWriter.Object);
        }

        [Test]
        public void Uploaded_File_Is_Saved_To_Server()
        {
            var sut = new CSVReaderWriterController(
                mockConfigManager.Object, mockLogger.Object, 
                mockNotifierEmail.Object, mockNotifierText.Object,
                mockContactFileReaderFactory.Object, mockContactFileWriterFactory.Object, 
                mockFileSystemHelper.Object
                );

            var mockControllerContext = new Mock<System.Web.Mvc.ControllerContext>();
            mockControllerContext.Setup(a => a.HttpContext.Server.MapPath(It.IsAny<string>())).Returns("full_path_to_file");
            sut.ControllerContext = mockControllerContext.Object;

            mockContactFileReader.Setup(a => a.GetContacts(It.IsAny<string>())).Returns(
                new List<ContactViewModel>
                {
                    new ContactViewModel(firstName:"firstName1", lastName:"lastName1", phoneNumber:"07123456789", email:"test1@test.com"),
                    new ContactViewModel(firstName:"firstName2", lastName:"lastName2", phoneNumber:"01234567890", email:"test2@test.com"),
                }
            );

            var sampleFileName = "sampleFileName.txt";
            var file = new Mock<HttpPostedFileBase>();

            file.Setup(x => x.ContentLength).Returns(sampleFileName.Length);
            file.Setup(x => x.FileName).Returns(sampleFileName);
            file.Setup(a => a.SaveAs(It.IsAny<string>())).Verifiable();

            var result = sut.Read(file.Object);

            file.Verify(a => a.SaveAs(It.IsAny<string>()), Times.AtLeast(1));
            Assert.IsNotNull(result);
        }


        [Test]
        public void Write_Generates_Model_With_Filename()
        {
            var sut = new CSVReaderWriterController(
                mockConfigManager.Object, mockLogger.Object, 
                mockNotifierEmail.Object, mockNotifierText.Object,
                mockContactFileReaderFactory.Object, mockContactFileWriterFactory.Object,
                mockFileSystemHelper.Object);

            var mockControllerContext = new Mock<System.Web.Mvc.ControllerContext>();
            mockControllerContext.Setup(a => a.HttpContext.Server.MapPath(It.IsAny<string>())).Returns("full_path_to_file");
            sut.ControllerContext = mockControllerContext.Object;

            var fileName = "test.txt";
            var result = (ViewResult)sut.Write(fileName);
            var model = (ContactViewModel)result.Model;
            Assert.IsNotNull(result);
            Assert.IsTrue(model.FileName == fileName);
        }

        [Test]
        public void Test_Email_Processing_Given_CSV_File_Uploaded()
        {
            var sut = new CSVReaderWriterController(
                mockConfigManager.Object, mockLogger.Object, 
                mockNotifierEmail.Object, mockNotifierText.Object,
                mockContactFileReaderFactory.Object, mockContactFileWriterFactory.Object,
                mockFileSystemHelper.Object);

            var mockControllerContext = new Mock<System.Web.Mvc.ControllerContext>();
            mockControllerContext.Setup(a => a.HttpContext.Server.MapPath(It.IsAny<string>())).Returns("full_path_to_file");
            sut.ControllerContext = mockControllerContext.Object;
            var fileName = "test.csv";

            var result = sut.Process(fileName);

            Assert.IsNotNull(result);
        }


        [Test]
        public void Write_Receives_Model_And_Appends_To_CSV_File()
        {
            mockContactFileWriter.Setup(a => a.AddContact(It.IsAny<string>(), It.IsAny<ContactViewModel>())).Verifiable();

            var sut = new CSVReaderWriterController(
                mockConfigManager.Object, mockLogger.Object, 
                mockNotifierEmail.Object, mockNotifierText.Object,
                mockContactFileReaderFactory.Object, mockContactFileWriterFactory.Object,
                mockFileSystemHelper.Object);

            var mockControllerContext = new Mock<System.Web.Mvc.ControllerContext>();
            mockControllerContext.Setup(a => a.HttpContext.Server.MapPath(It.IsAny<string>())).Returns("full_path_to_file");
            sut.ControllerContext = mockControllerContext.Object;

            

            var fileName = "csvFileName1.csv";
            var newContact = new ContactViewModel(
                firstName: "firstName", lastName: "lastName", phoneNumber: "0789123456", email: "test1@test.com", filename: fileName
                );
            var result = (ViewResult)sut.Write(newContact);
            var model = (DisplayInputViewModel)result.Model;

            mockContactFileWriter.Verify(a => a.AddContact(It.IsAny<string>(), It.IsAny<ContactViewModel>()), Times.AtLeast(1));
            Assert.IsNotNull(result);
            Assert.IsTrue(model.FileName == fileName);
        }



        [Test]
        public void Process_Sends_Email_Given_CSV_File_Uploaded()
        {
            mockContactFileReader.Setup(a => a.GetContacts(It.IsAny<string>())).Returns(
                new List<ContactViewModel>
                {
                    new ContactViewModel(firstName:"firstName1", lastName:"lastName1", phoneNumber:"07123456789", email:"test1@test.com"),
                    new ContactViewModel(firstName:"firstName2", lastName:"lastName2", phoneNumber:"01234567890", email:"test2@test.com"),
                }
            );

            var mockControllerContext = new Mock<System.Web.Mvc.ControllerContext>();
            mockControllerContext.Setup(a => a.HttpContext.Server.MapPath(It.IsAny<string>())).Returns("full_path_to_file");
            var fileName = "test.csv";

            // setup container for mock and non-mock components
            mockEmailClient.Setup(a => a.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var notifierEmail = new NotifierEmail(mockConfigManager.Object, mockEmailClient.Object, mockLogger.Object);
            var container = new UnityContainer();
            SetupContainer(container,
                mockConfigManager.Object, mockEmailClient.Object, mockLogger.Object, mockContactFileReader.Object,
                notifierEmail, // this should throw exception when using mockEmailClient
                mockNotifierText.Object,
                mockContactFileReaderFactory.Object, mockContactFileWriterFactory.Object, 
                mockFileSystemHelper.Object);

            // get sut
            var sut = container.Resolve<CSVReaderWriterController>();
            sut.ControllerContext = mockControllerContext.Object;
            
            var result = sut.Process(fileName);

            Assert.IsNotNull(result);
            mockEmailClient.Verify(a => a.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(2));
            mockLogger.Verify(a => a.Log(It.IsAny<string>(), LogLevel.Info), Times.AtLeast(2));
        }

        [Test]
        public void ResolveController_CSVReaderWriterController()
        {
            var container = ContactProcessor.App_Start.UnityConfig.GetConfiguredContainer();
            var csvReaderWriterController = container.Resolve<CSVReaderWriterController>();

            Assert.IsNotNull(csvReaderWriterController);
            Assert.IsNotNull(csvReaderWriterController.GetType().Name == nameof(CSVReaderWriterController));
        }

        [Test]
        public void Email_Sending_Error_Gets_Logged()
        {
            mockContactFileReader.Setup(a => a.GetContacts(It.IsAny<string>())).Returns(
                new List<ContactViewModel>
                {
                                new ContactViewModel(firstName:"firstName1", lastName:"lastName1", phoneNumber:"07123456789", email:"test1@test.com"),
                                new ContactViewModel(firstName:"firstName2", lastName:"lastName2", phoneNumber:"01234567890", email:"test2@test.com"),
                }
            );
            mockEmailClient.Setup(a => a.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new System.Exception("Error occured"));
            mockLogger.Setup(a => a.Log(It.IsAny<string>(), It.IsAny<LogLevel>())).Verifiable();
            var notifierEmail = new NotifierEmail(mockConfigManager.Object, mockEmailClient.Object, mockLogger.Object);

            var mockControllerContext = new Mock<System.Web.Mvc.ControllerContext>();
            mockControllerContext.Setup(a => a.HttpContext.Server.MapPath(It.IsAny<string>())).Returns("full_path_to_file");
            var fileName = "test.csv";
            
            // setup container for mock and non-mock components
            var container = new UnityContainer();
            SetupContainer(container,
                mockConfigManager.Object, mockEmailClient.Object, mockLogger.Object, mockContactFileReader.Object,
                notifierEmail, // this should throw exception when using mockEmailClient
                mockNotifierText.Object,
                mockContactFileReaderFactory.Object, mockContactFileWriterFactory.Object, 
                mockFileSystemHelper.Object);

            // get sut
            var sut = container.Resolve<CSVReaderWriterController>();
            sut.ControllerContext = mockControllerContext.Object;


            var result = sut.Process(fileName);

            Assert.IsNotNull(result);
            mockLogger.Verify(a => a.Log(It.IsAny<string>(), LogLevel.Error), Times.AtLeast(2));
        }


        public void SetupContainer(IUnityContainer container,
            IConfigManager configManager, IEmailClient emailClient, ILogger logger, IContactFileReader contactFileReader,
            INotifier notifierEmail, INotifier notifierText,
            IContactFileReaderFactory contactFileReaderFactory, IContactFileWriterFactory contactFileWriterFactory, 
            IFileSystemHelper fileSystemHelper
            )
        {
            container.RegisterInstance<IConfigManager>(configManager);
            container.RegisterInstance<IEmailClient>(emailClient);
            container.RegisterInstance<ILogger>(logger);
            container.RegisterInstance<IContactFileReader>(contactFileReader);

            container.RegisterInstance<INotifier>(Utilities.Constants.IOC_NotifierEmail, notifierEmail);
            container.RegisterInstance<INotifier>(Utilities.Constants.IOC_NotifierText, notifierText);

            container.RegisterInstance<IContactFileReaderFactory>(contactFileReaderFactory);
            container.RegisterInstance<IContactFileWriterFactory>(contactFileWriterFactory);
            
            container.RegisterInstance<IFileSystemHelper>(fileSystemHelper);
            
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

        /*
         * Add tests for:
         * Input data is appended to the file.
         / Process sends emails
         / When email sending fails, error gets logged.
         * Add tests for interface implementations.
         */

    }
}

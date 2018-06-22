using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContactProcessor.Models;
using ContactProcessor.Utilities;
using ContactProcessor.Utilities.ConfigManager;
using ContactProcessor.Utilities.ContactFileReader;
using ContactProcessor.Utilities.ContactFileWriter;
using ContactProcessor.Utilities.FileSystemHelper;
using ContactProcessor.Utilities.Logger;
using ContactProcessor.Utilities.Notifier;

namespace ContactProcessor.Controllers
{
    public class CSVReaderWriterController : Controller
    {
        private readonly IConfigManager _configManager;
        private readonly ILogger _logger;

        private readonly INotifier _notifierEmail;
        private readonly INotifier _notifierText;
        
        private readonly IContactFileReaderFactory _contactFileReaderFactory;
        private readonly IContactFileWriterFactory _contactFileWriterFactory;
        private readonly IFileSystemHelper _fileSystemHelper;

        public CSVReaderWriterController(IConfigManager configManager, ILogger logger,
            INotifier notifierEmail, INotifier notifierText,
            IContactFileReaderFactory contactFileReaderFactory, IContactFileWriterFactory contactFileWriterFactory,
            IFileSystemHelper fileSystemHelper
            )
        {
            _configManager = configManager;
            _logger = logger;

            _notifierEmail = notifierEmail;
            _notifierText = notifierText;

            _contactFileReaderFactory = contactFileReaderFactory;
            _contactFileWriterFactory = contactFileWriterFactory;

            _fileSystemHelper = fileSystemHelper;
        }

        /// <summary>
        /// Shows file upload page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Read()
        {
            return View();
        }

        /// <summary>
        /// Reads data from csv file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Read(HttpPostedFileBase file)
        {
            if (file.ContentLength == 0)
                return View();
            
            var fileName = _fileSystemHelper.GenerateUniqueFileName(file.FileName); 
            var fileFullPath = _fileSystemHelper.GetFullPath(fileName);

            _fileSystemHelper.EnsureDirectoryExists(fileFullPath);
            
            file.SaveAs(fileFullPath);

            var model = new DisplayInputViewModel();
            model.FileName = fileName;
            model.Contacts = _fileSystemHelper.GetContactsFromFile(fileFullPath);

            return View("DisplayInput", model);
        }

        /// <summary>
        /// Shows the form for users input for new contact entry
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Write(string filename)
        {
            var model = new ContactViewModel(filename: filename);
            return View(model);
        }

        /// <summary>
        /// new contact entry gets added to the file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Write(ContactViewModel model)
        {
            var fileFullPath = _fileSystemHelper.GetFullPath(model.FileName);
            using (var writer = _contactFileWriterFactory.Create())
            {
                writer.AddContact(fileFullPath, model);
            }
                        
            var data = new DisplayInputViewModel();
            data.FileName = model.FileName;
            data.Contacts = _fileSystemHelper.GetContactsFromFile(fileFullPath);
            
            return View("DisplayInput", data);
        }

        /// <summary>
        /// Sends emails to the emails which are read from CSV file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ActionResult Process(string filename)
        {
            using (var contactFileReader = _contactFileReaderFactory.Create())
            {
                var fileFullPath = _fileSystemHelper.GetFullPath(filename);
                Parallel.ForEach(contactFileReader.GetContacts(fileFullPath), contact =>
                {
                    if (_notifierEmail.CanNotify(contact))
                    {
                        _notifierEmail.Notify(contact);
                    }
                    else if (_notifierText.CanNotify(contact))
                    {
                        _notifierText.Notify(contact);
                    }
                    else
                    {
                        _logger.Log($"Cannot process contact: {contact.ToString()}", LogLevel.Error);
                    }
                });
            }
            
            var emailSentMessage = _configManager.Get(Constants.AppConfigKey_EmailSentMessage);
            return Content(emailSentMessage);
        }

        
    }
}
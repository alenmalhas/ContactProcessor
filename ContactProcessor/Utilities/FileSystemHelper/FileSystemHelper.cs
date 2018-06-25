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
using ContactProcessor.Utilities.Logger;
using ContactProcessor.Utilities.Notifier;


namespace ContactProcessor.Utilities.FileSystemHelper
{
    public class FileSystemHelper:IFileSystemHelper
    {
        private readonly IConfigManager _configManager;
        private readonly IContactFileReaderFactory _contactFileReaderFactory;

        public FileSystemHelper(IConfigManager configManager, IContactFileReaderFactory contactFileReaderFactory )
        {
            _configManager = configManager;
            _contactFileReaderFactory = contactFileReaderFactory;
        }

        public string _uploadFolderFullPath
        {
            get
            {
                var uploadFolderRelativePath = _configManager.Get(Constants.AppConfigKey_fileUploadFolderRelativePath);
                var uploadFolderFullpath = System.Web.Hosting.HostingEnvironment.MapPath(uploadFolderRelativePath);
                return uploadFolderFullpath;
            }
        }
        public string GetFullPath(string fileName)
        {
            var fileFullPath = Path.Combine(_uploadFolderFullPath, fileName);
            return fileFullPath;
        }

        public List<ContactViewModel> GetContactsFromFile(string fileFullPath)
        {
            List<ContactViewModel> contacts;
            using (var reader = _contactFileReaderFactory.Create())
            {
                contacts = reader.GetContacts(fileFullPath).ToList();
            }
            return contacts;
        }

        public string GenerateUniqueFileName(string uploadedFileName)
        {
            var fileName = Path.GetFileName(uploadedFileName);
            var splitfilename = fileName.Split('.').ToArray();
            var uniqueFileNameOnServer = splitfilename[0] + Guid.NewGuid() + '.' + splitfilename[1];

            return uniqueFileNameOnServer;
        }
     
        public void EnsureDirectoryExists(string fileFullPath)
        {
            // check if upload folder exists, if not create it.
            if (!Directory.Exists(Directory.GetParent(fileFullPath).FullName))
                Directory.CreateDirectory(Directory.GetParent(fileFullPath).FullName);
        }

    }
}
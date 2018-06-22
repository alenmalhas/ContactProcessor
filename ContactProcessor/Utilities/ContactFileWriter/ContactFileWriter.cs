using ContactProcessor.Models;
using ContactProcessor.Utilities.ConfigManager;
using ContactProcessor.Utilities.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities.ContactFileWriter
{
    public class ContactFileWriter : IContactFileWriter
    {
        private ILogger _logger;
        private IConfigManager _configManager;

        public ContactFileWriter(ILogger logger, IConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
        }

        public void AddContact(string fileFullPath, ContactViewModel contact)
        {
            System.IO.File.AppendAllText(fileFullPath, contact.ToString());
        }


        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed;
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {   // Free any other managed objects here.
                _logger = null;
                _configManager = null;
            }
            // We don't have any unmanaged objects to dispose.

            disposed = true;
        }


        #endregion IDisposable implementation

    }
}
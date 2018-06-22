using ContactProcessor.Models;
using ContactProcessor.Utilities.ConfigManager;
using ContactProcessor.Utilities.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities.ContactFileReader
{
    public class ContactFileReader : IContactFileReader
    {
        private ILogger _logger;
        private IConfigManager _configManager;

        public ContactFileReader(ILogger logger, IConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
        }

        public IEnumerable<ContactViewModel> GetContacts(string fileFullPath)
        {
            using (var reader = new StreamReader(fileFullPath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (!line.Contains(','))
                    {
                        _logger.Log($"Line doesn't contain comma: {line}", LogLevel.Error);
                        continue;
                    }

                    var values = line.Split(',');
                    if (values.Length > 3)
                    {
                        yield return new ContactViewModel(
                            firstName: values[0],
                            lastName: values[1],
                            phoneNumber: values[2],
                            email: values[3]);
                    }
                    else
                    {
                        _logger.Log($"Invalid line: {line}", LogLevel.Error);
                    }
                }
            }
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
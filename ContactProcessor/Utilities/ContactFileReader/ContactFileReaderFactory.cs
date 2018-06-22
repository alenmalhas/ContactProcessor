using ContactProcessor.Utilities.ConfigManager;
using ContactProcessor.Utilities.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities.ContactFileReader
{
    public class ContactFileReaderFactory : IContactFileReaderFactory
    {
        private readonly ILogger _logger;
        private readonly IConfigManager _configManager;

        public ContactFileReaderFactory(ILogger logger, IConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
        }

        public IContactFileReader Create()
        {
            return new ContactFileReader(_logger, _configManager);
        }
    }
}
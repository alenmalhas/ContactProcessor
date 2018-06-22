using ContactProcessor.Utilities.ConfigManager;
using ContactProcessor.Utilities.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities.ContactFileWriter
{
    public class ContactFileWriterFactory : IContactFileWriterFactory
    {
        private readonly ILogger _logger;
        private readonly IConfigManager _configManager;

        public ContactFileWriterFactory(ILogger logger, IConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
        }

        public IContactFileWriter Create()
        {
            return new ContactFileWriter(_logger, _configManager);
        }
    }
}
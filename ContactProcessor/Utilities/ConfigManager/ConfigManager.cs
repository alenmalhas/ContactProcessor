using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities.ConfigManager
{
    public class ConfigManager : IConfigManager
    {
        public string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
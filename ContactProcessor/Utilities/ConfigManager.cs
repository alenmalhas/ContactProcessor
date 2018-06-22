using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities
{
    public interface IConfigManager
    {
        string Get(string key);
    }

    public class ConfigManager : IConfigManager
    {
        public string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
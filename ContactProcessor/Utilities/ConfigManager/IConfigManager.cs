using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactProcessor.Utilities.ConfigManager
{
    public interface IConfigManager
    {
        string Get(string key);
    }
}

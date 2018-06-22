using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactProcessor.Utilities.Logger
{
    public enum LogLevel
    {
        Debug, Info, Error
    }

    public interface ILogger
    {
        void Log(string message, LogLevel logLevel);
    }
}
